[assembly: Microsoft.Owin.OwinStartup(typeof(JodosServer.Startup))]

namespace JodosServer
{
    using AspNet.Identity.MongoDB;
    using Autofac;
    using Autofac.Builder;
    using Autofac.Core;
    using Autofac.Integration.WebApi;
    using Entities;
    using Microsoft.AspNet.Identity;
    using Microsoft.Owin;
    using Microsoft.Owin.Security.Infrastructure;
    using Microsoft.Owin.Security.OAuth;
    using Owin;
    using Providers;
    using System;
    using System.Web.Http;
    using JodosServer;
    using JodosServer.Repositories;
    using System.Collections.Generic;
    using JodosServer.Models;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;
    using System.Text;
    using System.Threading;
    using System.Web.Script.Serialization;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MongoContext>().AsImplementedInterfaces<MongoContext, ConcreteReflectionActivatorData>().SingleInstance();

            builder.RegisterType<AuthRepository>().SingleInstance();

            builder.RegisterType<ItemsRepository>().SingleInstance();

            builder.RegisterType<ApplicationIdentityContext>()
                .SingleInstance();

            builder.RegisterType<UserStore<User>>()
                .AsImplementedInterfaces<IUserStore<User>, ConcreteReflectionActivatorData>()
                .SingleInstance();

            builder.RegisterType<RoleStore<Role>>()
                .AsImplementedInterfaces<IRoleStore<Role>, ConcreteReflectionActivatorData>()
                .SingleInstance();

            builder.RegisterType<ApplicationUserManager>()
                .SingleInstance();

            builder.RegisterType<ApplicationRoleManager>()
                .SingleInstance();

            builder.RegisterType<SimpleAuthorizationServerProvider>()
                .AsImplementedInterfaces<IOAuthAuthorizationServerProvider, ConcreteReflectionActivatorData>().SingleInstance();

            builder.RegisterType<SimpleRefreshTokenProvider>()
                .AsImplementedInterfaces<IAuthenticationTokenProvider, ConcreteReflectionActivatorData>().SingleInstance();

            builder.RegisterApiControllers(typeof(Startup).Assembly);

            var container = builder.Build();

            app.UseAutofacMiddleware(container);

            var webApiDependencyResolver = new AutofacWebApiDependencyResolver(container);

            var configuration = new HttpConfiguration
            {
                DependencyResolver = webApiDependencyResolver
            };

            ConfigureOAuth(app, container);

            WebApiConfig.Register(configuration);

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            app.UseWebApi(configuration);

            app.UseAutofacWebApi(configuration);

            InitializeData(container);
        }


        public static void ReadRabbit(object mongoContext)
        {
            
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare("hello", false, false, false, null);

                    var consumer = new QueueingBasicConsumer(channel);
                    channel.BasicConsume("hello", true, consumer);

                    while (true)
                    {
                        var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();

                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);

                        JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                        Result objResult = jsonSerializer.Deserialize<Result>(message);

                        //insert json to mongo here
                        ((IMongoContext)mongoContext).Results.Insert(new Result
                        {
                            url = objResult.url,
                            user = objResult.user,
                            sellsite = "" //objResult.sellsite
                        });

                    }
                }
            }
        }


        private void ConfigureOAuth(IAppBuilder app, IContainer container)
        {
            var OAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                Provider = container.Resolve<IOAuthAuthorizationServerProvider>(),
                RefreshTokenProvider = container.Resolve<IAuthenticationTokenProvider>()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);

            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }

        private void InitializeData(IContainer container)
        {
            var mongoContext = container.Resolve<IMongoContext>();

            Thread thread = new Thread(new ParameterizedThreadStart(ReadRabbit));

            thread.Start(mongoContext);

        }
    }
}