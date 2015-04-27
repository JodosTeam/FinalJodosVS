using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JodosServer.Models.Amazon
{
    public class Name
    {
        public string text { get; set; }
        public string href { get; set; }
    }

    public class Pic
    {
        public string text { get; set; }
        public string href { get; set; }
        public string alt { get; set; }
        public string src { get; set; }
    }

    public class Price
    {
        public string text { get; set; }
        public string href { get; set; }
    }

    public class Collection1
    {
        public Name name { get; set; }
        public Pic pic { get; set; }
        public Price price { get; set; }
        public string freeship { get; set; }
    }

    public class Results
    {
        public List<Collection1> collection1 { get; set; }
    }

    public class RootObject
    {
        public string name { get; set; }
        public int count { get; set; }
        public string url { get; set; }
        public Results results { get; set; }
    }
}