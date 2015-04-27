using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JodosServer.Models.Ali
{
    public class Property2
    {
        public string text { get; set; }
        public string href { get; set; }
    }

    public class Collection1
    {
        public Property2 property2 { get; set; }
        public string property3 { get; set; }
        public string property4 { get; set; }
        public string property5 { get; set; }
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