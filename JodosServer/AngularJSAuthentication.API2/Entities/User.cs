namespace JodosServer.Entities
{
    using AspNet.Identity.MongoDB;
    using MongoDB.Bson.Serialization.Attributes;
    using System;
    using System.Collections.Generic;

    public class User : IdentityUser
    {
        public DateTime CreateDate { get; set; }

        [BsonIgnoreIfNull]
        public List<string> Mataks { get; set; }

        public string FirstName { get; set; }
        
        public string LastName { get; set; }

    }
}