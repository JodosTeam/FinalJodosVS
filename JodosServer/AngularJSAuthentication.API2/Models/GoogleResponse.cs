using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JodosServer.Models.Google
{
    public class Item
    {
        public string link { get; set; }
    }

    public class RootObject
    {
        public List<Item> items { get; set; }
    }
}