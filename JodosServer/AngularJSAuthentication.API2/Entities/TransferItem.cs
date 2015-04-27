namespace JodosServer.Entities
{
    using Models;
    using MongoDB.Bson.Serialization.Attributes;
    using System.ComponentModel.DataAnnotations;

    public class TransferItem
    {
        public string Name { get; set; }

        public string Price { get; set; }

        public string Link { get; set; }

        public string Source { get; set; }
    }
}