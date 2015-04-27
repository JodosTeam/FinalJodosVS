using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JodosServer.Models.Ali
{
    public class Product
    {
        public string imageUrl { get; set; }
        public string productUrl { get; set; }
        public string productTitle { get; set; }
        public string salePrice { get; set; }
    }

    public class Result
    {
        public int totalResults { get; set; }
        public List<Product> products { get; set; }
    }

    public class RootObject
    {
        public Result result { get; set; }
        public int errorCode { get; set; }
    }
}