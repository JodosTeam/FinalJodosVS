namespace JodosServer.Repositories
{
    using JodosServer.Entities;
    using JodosServer.Models;
    using Microsoft.AspNet.Identity;
    using MongoDB.Driver;
    using MongoDB.Driver.Linq;
    using MongoDB.Driver.Builders;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using JodosServer.DTOs;

    public class AuthRepository
    {
        private readonly IMongoContext mongoContext;
        private readonly ApplicationUserManager userManager;

        public AuthRepository(IMongoContext mongoContext, ApplicationUserManager userManager)
        {
            this.mongoContext = mongoContext;
            this.userManager = userManager;
        }

        public async Task<IdentityResult> RegisterUser(UserModel userModel)
        {
            var user = new User
            {
                UserName = userModel.UserName,
                CreateDate = DateTime.Now,
                FirstName = userModel.firstName,
                LastName = userModel.lastName
            };
            user.Roles.Add("User");

            var result = await userManager.CreateAsync(user, userModel.Password);

            return result;
        }

        public WriteConcernResult RemoveUser(string userName)
        {
            var query = Query<User>.EQ(e => e.UserName, userName);
            WriteConcernResult results = mongoContext.Users.Remove(query);
            
            return results;
        }

        public async Task<User> FindUser(string userName, string password)
        {
            User user = await userManager.FindAsync(userName, password);

            return user;
        }

        public async Task<IdentityResult> AddUserToRole(string userName, string roleName)
        {
            User user = await userManager.FindByNameAsync(userName);

            var result = await userManager.AddToRoleAsync(user.Id, roleName);
            
            return result;
        }

        public UsersAndRolesDTO getAllUsersAndRoles()
        {
            List<UserDTO> users = new List<UserDTO>();
            List<Role> roles = mongoContext.Roles.FindAll().ToList();

            foreach (User user in userManager.Users.ToList())
                users.Add(new UserDTO(user));

            UsersAndRolesDTO dto = new UsersAndRolesDTO(users, roles);

            return dto;
        }

        public Client FindClient(string clientId)
        {
            var query = Query<Client>.EQ(c => c.Id, clientId);

            var client = mongoContext.Clients.Find(query).SetLimit(1).FirstOrDefault();

            return client;
        }

        public async Task<bool> AddRefreshToken(RefreshToken token)
        {
            var query = Query.And(
                Query<RefreshToken>.EQ(r => r.Subject, token.Subject),
                Query<RefreshToken>.EQ(r => r.ClientId, token.ClientId));

            var existingToken = mongoContext.RefreshTokens.Find(query).SetLimit(1).SingleOrDefault();

            if (existingToken != null)
            {
                var result = await RemoveRefreshToken(existingToken);
            }

            mongoContext.RefreshTokens.Insert(token);

            return true;
        }

        public Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            var query = Query<RefreshToken>.EQ(r => r.Id, refreshTokenId);

            var writeConcernResult = mongoContext.RefreshTokens.Remove(query);

            return Task.FromResult(writeConcernResult.DocumentsAffected == 1);
        }

        public async Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
        {
            return await RemoveRefreshToken(refreshToken.Id);
        }

        public Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {   
            var query = Query<RefreshToken>.EQ(r => r.Id, refreshTokenId);

            var refreshToken = mongoContext.RefreshTokens.Find(query).SetLimit(1).FirstOrDefault();

            return Task.FromResult(refreshToken);
        }

        public List<RefreshToken> GetAllRefreshTokens()
        {
            return mongoContext.RefreshTokens.FindAll().ToList();
        }
    }
}