using JodosServer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Text;
using System.IO;

namespace JodosServer.Controllers
{
    [RoutePrefix("api/Items")]
    public class ItemsController : ApiController
    {
        private readonly ItemsRepository itemsRepository = null;

        public ItemsController(ItemsRepository itemsRepository)
        {
            this.itemsRepository = itemsRepository;
        }

        //[Route("getItems")]
        public IHttpActionResult Get()//[FromBody] string searchText)
        {
            string searchText = "iPhone";
            return Ok(itemsRepository.searchItems(searchText));
        }

        [HttpPost]
        //[Route("or")]
        public IHttpActionResult SetShopSubCategories([FromBody]string searchText)
        {
            return Ok(itemsRepository.searchItems(searchText));
        }

        [HttpPost]
        [Route("google")]
        public IHttpActionResult SearchGoogle([FromBody]string searchText)
        {
            itemsRepository.SearchGoogle(searchText);
            return Ok();
        }

       
    }
}
