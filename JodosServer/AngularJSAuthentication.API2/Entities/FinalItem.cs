using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JodosServer.Entities
{
    public class FinalItem
    {
        public string Name          { get; set; }

        public string ItemUrl       { get; set; }

        public string ImageUrl      { get; set; }

        public string    ItemPrice     { get; set; }

        public string    ShippingPrice { get; set; }

        public string Source { get; set; }
    }
}