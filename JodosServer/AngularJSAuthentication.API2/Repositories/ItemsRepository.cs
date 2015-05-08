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
    using System.Text;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    public class ItemsRepository
    {
        private readonly IMongoContext mongoContext;
        private static List<FinalItem> FinalList;
        public delegate long StatisticalData();

        public ItemsRepository(IMongoContext mongoContext)
        {
            this.mongoContext = mongoContext;
        }

        public void SearchGoogle(string searchText)
        {
            string username = "dani";
            string url = "https://www.googleapis.com/customsearch/v1?key=AIzaSyAwFxWoXwWNts6fyZpN3cowCb5BXoL0qT4&cx=017135603890338635452:l5ri3atpm-y&q=" +"buy "+searchText + "&fields=items/link";

            System.Net.WebRequest req = System.Net.WebRequest.Create(url);
            req.Proxy = WebProxy.GetDefaultProxy();
            System.Net.WebResponse resp = req.GetResponse();
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());

            JsonSerializerSettings x = new JsonSerializerSettings();
            x.MissingMemberHandling = MissingMemberHandling.Ignore;

            JodosServer.Models.Google.RootObject retValue = JsonConvert.DeserializeObject<JodosServer.Models.Google.RootObject>(sr.ReadToEnd(), x);
            List<JodosServer.Models.Google.Item> googleItems = retValue.items;

            StringBuilder sd = new StringBuilder();
            
            foreach (var item in googleItems)
            {

                string Push = "{\"url\":" + "\"" + item.link + "\"" + ",\"user\":" + "\"" + username + "\"" + "}";
                PushRabbit(Push);
                PushRabbit2(item.link);
            }

        }

        public static void PushRabbit(string message)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare("hello", false, false, false, null);

                    
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish("", "hello", null, body);
                }
            }
        }

        public static void PushRabbit2(string message)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //channel.QueueDeclare("TEST1", false, false, false, null);


                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish("", "TEST1", null, body);
                }
            }
        }

        public static void ReadRabbit()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare("hello", false, false, false, null);

                    var consumer = new QueueingBasicConsumer(channel);
                    channel.BasicConsume("hello", true, consumer);

                    while (true)
                    {
                        var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();

                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        
                    }
                }
            }
        }

        

        public List<FinalItem> searchItems(string searchText)
        {

            callAli("iphone");
            FinalList = new List<FinalItem>();
            List<Thread> threads = new List<Thread>();

            //threads.Add(new Thread(new ParameterizedThreadStart(ItemsRepository.callAli)));
            threads.Add(new Thread(new ParameterizedThreadStart(ItemsRepository.callEbay)));
            threads.Add(new Thread(new ParameterizedThreadStart(ItemsRepository.callAli)));



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



        private static void callAli(object textIn)
        {
            string searchText = textIn.ToString();

            searchText = System.Web.HttpUtility.HtmlEncode(searchText);

            string url = "http://gw.api.alibaba.com/openapi/param2/2/portals.open/api.listPromotionProduct/71307?"
             + "keywords=" + searchText
             + "&fields=productTitle,productUrl,salePrice,imageUrl"
             + "&pageSize=10&sort=sellerRateDown";

            System.Net.WebRequest req = System.Net.WebRequest.Create(url);
            req.Proxy = WebProxy.GetDefaultProxy();
            System.Net.WebResponse resp = req.GetResponse();
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());

            JsonSerializerSettings x = new JsonSerializerSettings();
            x.MissingMemberHandling = MissingMemberHandling.Ignore;

            JodosServer.Models.Ali.RootObject retValue = JsonConvert.DeserializeObject<JodosServer.Models.Ali.RootObject>(sr.ReadToEnd(), x);
            List<JodosServer.Models.Ali.Product> alitems = retValue.result.products;

            foreach (var item in alitems)
            {
                
            }
        }
    }
}