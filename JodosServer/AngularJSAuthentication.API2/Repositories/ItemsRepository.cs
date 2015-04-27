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
    using System.Net;
    using Newtonsoft.Json;
    using System.Threading;

    public class ItemsRepository
    {
        private readonly IMongoContext mongoContext;
        private static List<FinalItem> FinalList;
        public delegate long StatisticalData();

        public ItemsRepository(IMongoContext mongoContext)
        {
            this.mongoContext = mongoContext;
        }

        public List<FinalItem> searchItems(string searchText)
        {
            FinalList = new List<FinalItem>();
            List<Thread> threads = new List<Thread>();

            //threads.Add(new Thread(new ParameterizedThreadStart(ItemsRepository.callAli)));
            threads.Add(new Thread(new ParameterizedThreadStart(ItemsRepository.callEbay)));
            threads.Add(new Thread(new ParameterizedThreadStart(ItemsRepository.callAmazon)));
       
         //   t1.Start(searchText);
          //  t2.Start(searchText);
        
          //  t1.Join();
          //  t2.Join();


            foreach (var thread in threads)
            {
                thread.Start(searchText);
            }
            foreach (var thread in threads)
            {
                thread.Join();
            }
        
            return FinalList;
        }

        private static void callAmazon(object textIn)
        {
            string searchText = System.Web.HttpUtility.HtmlEncode(textIn);

            string url = "https://www.kimonolabs.com/api/ondemand/20l8pw8y?apikey=2JUxiAvx1KNCoUrS7tSqmd18WXDG6eFo&field-keywords=";
            url += searchText;


            System.Net.WebRequest req = System.Net.WebRequest.Create(url);
            req.Proxy = WebProxy.GetDefaultProxy();
            System.Net.WebResponse resp = req.GetResponse();
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());

            JsonSerializerSettings x = new JsonSerializerSettings();
            x.MissingMemberHandling = MissingMemberHandling.Ignore;

            JodosServer.Models.Amazon.RootObject retValue = JsonConvert.DeserializeObject<JodosServer.Models.Amazon.RootObject>(sr.ReadToEnd(), x);

            List<JodosServer.Models.Amazon.Collection1> aliItems = retValue.results.collection1;

            foreach (var item in aliItems)
            {
                if (item.name.href != string.Empty)
                {
                    FinalItem temp = new FinalItem();

                    temp.ItemUrl = item.name.href;
                    temp.ImageUrl = item.pic.src;
                    temp.ItemPrice = item.price.text;
                    if (item.freeship != string.Empty)
                        temp.ShippingPrice = "0";
                    else
                        temp.ShippingPrice = "Unknown";
                    temp.Name = item.name.text;
                    temp.Source = "Amazon";
                    FinalList.Add(temp);
                }
            }
        }

        private static void callEbay(object textIn)
        {
            string text = textIn.ToString();
            text = System.Web.HttpUtility.HtmlEncode(text);

            string url = "http://svcs.ebay.com/services/search/FindingService/v1";
            url += "?OPERATION-NAME=findItemsByKeywords";
            url += "&SERVICE-VERSION=1.0.0";
            url += "&SECURITY-APPNAME=JODOS-TE-0e9b-4bbc-9101-1c79d952427c";
            url += "&GLOBAL-ID=EBAY-US";
            url += "&RESPONSE-DATA-FORMAT=JSON";
            url += "&keywords=" + text;
            url += "&paginationInput.entriesPerPage=10";
            url += "&SortOrder=PricePlusShippingLowest";
            url += "&itemFilter(0).name=ListingType";
            url += "&itemFilter(0).value=FixedPrice";
            url += "&itemFilter(1).name=Condition";
            url += "&itemFilter(1).value=New";
            url += "&itemFilter(2).name=TopRatedSellerOnly";
            url += "&itemFilter(2).value=true";
            url += "&itemFilter(3).name=AvailableTo";
            url += "&itemFilter(3).value=IL";
            
            

            System.Net.WebRequest req = System.Net.WebRequest.Create(url);
            req.Proxy = WebProxy.GetDefaultProxy();
            System.Net.WebResponse resp = req.GetResponse();
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());

            JsonSerializerSettings x = new JsonSerializerSettings();
            x.MissingMemberHandling = MissingMemberHandling.Ignore;

            EbayResponse retValue = JsonConvert.DeserializeObject<EbayResponse>(sr.ReadToEnd(), x);
            if (retValue.findItemsByKeywordsResponse[0].ack[0].ToString() != "Failure")
            {
                IList<Item> ebayItems = retValue.findItemsByKeywordsResponse[0].searchResult[0].item;

                foreach (Item item in ebayItems)
                {
                    FinalItem temp = new FinalItem();

                    temp.ItemUrl = item.viewItemURL[0].ToString();
                    temp.ImageUrl = item.galleryURL[0].ToString();
                    temp.ItemPrice = item.sellingStatus[0].currentPrice[0].__value__;
                    if (item.shippingInfo[0].shippingServiceCost == null)
                        temp.ShippingPrice = "0";
                    else
                        temp.ShippingPrice = item.shippingInfo[0].shippingServiceCost[0].__value__;
                    temp.Name = item.title[0].ToString();
                    temp.Source = "eBay";
                    FinalList.Add(temp);
                }
            }
        }

        public static void callAli(object text)
        {
            string searchText = text.ToString();
            searchText = System.Web.HttpUtility.HtmlEncode(searchText);

            string url = "https://www.kimonolabs.com/api/ondemand/ais11kdy?apikey=2JUxiAvx1KNCoUrS7tSqmd18WXDG6eFo&SearchText=";
            url += searchText;


            System.Net.WebRequest req = System.Net.WebRequest.Create(url);
            req.Proxy = WebProxy.GetDefaultProxy();
            System.Net.WebResponse resp = req.GetResponse();
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());

            JsonSerializerSettings x = new JsonSerializerSettings();
            x.MissingMemberHandling = MissingMemberHandling.Ignore;

            JodosServer.Models.Ali.RootObject retValue = JsonConvert.DeserializeObject<JodosServer.Models.Ali.RootObject>(sr.ReadToEnd(), x);

            List<JodosServer.Models.Ali.Collection1> aliItems = retValue.results.collection1;

            foreach (var item in aliItems)
            {
                if (item.property2.href != string.Empty)
                {
                    FinalItem temp = new FinalItem();

                    temp.ItemUrl = item.property2.href;
                    temp.ImageUrl = "TODO: Add picture";
                    temp.ItemPrice = item.property3;
                    if (item.property4 != string.Empty)
                        temp.ShippingPrice = "0";
                    else
                        temp.ShippingPrice = item.property5;
                    temp.Name = item.property2.text;
                    temp.Source = "Ali";
                    FinalList.Add(temp);

                }
            }

        }
    }
}