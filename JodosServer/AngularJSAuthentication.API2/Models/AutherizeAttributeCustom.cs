using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using System.Configuration;
using JodosServer.Entities;

namespace JodosServer.Models
{
    public class AutherizeAttributeCustom : AuthorizeAttribute
    {
        protected override bool IsAuthorized(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
               System.Collections.ObjectModel.Collection<AutherizeAttributeCustom> authorizeAttributes = actionContext.ActionDescriptor.GetCustomAttributes<AutherizeAttributeCustom>();
               
               foreach (AutherizeAttributeCustom attribute in authorizeAttributes)
               {
                   if (userInRoles(HttpContext.Current.User.Identity.Name, attribute.Roles.Split(',')))
                       return true;
               }
           }
           return false;
        }

        private bool userInRoles (string username, string[] roles)
        {
            var mongoUrlBuilder = new MongoUrlBuilder(ConfigurationManager.ConnectionStrings["AuthContext"].ConnectionString);

            var mongoClient = new MongoClient(mongoUrlBuilder.ToMongoUrl());
            var server = mongoClient.GetServer();

            MongoDatabase Database  = server.GetDatabase(mongoUrlBuilder.DatabaseName);

            MongoCollection<User> userCollection = Database.GetCollection<User>("users");

            var result = userCollection.AsQueryable<User>().Where(a => a.UserName == username);

            foreach (User user in result)
            {
                return user.Roles.Intersect(roles).Any();
            }
                
            return false;
        }
    }
}