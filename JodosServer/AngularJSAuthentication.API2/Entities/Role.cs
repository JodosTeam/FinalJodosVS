namespace JodosServer.Entities
{
    using JodosServer.Models;
    using AspNet.Identity.MongoDB;
    using MongoDB.Bson.Serialization.Attributes;
    using System.Collections.Generic;

    public class Role : IdentityRole
    {
        public string Value { get; set;}
    }
}
