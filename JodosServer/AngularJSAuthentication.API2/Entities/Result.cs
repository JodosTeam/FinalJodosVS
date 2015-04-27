namespace JodosServer.Entities
{
    using JodosServer.Models;
    using AspNet.Identity.MongoDB;
    using MongoDB.Bson.Serialization.Attributes;
    using System.Collections.Generic;

    public class Result 
    {
        public string url { get; set;}
        public string user { get; set; }
        public string sellsite { get; set; }
    }
}
