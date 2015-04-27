using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JodosServer.Models
{
    public class ReportsSearchModel
    {
        public string AgraId { get; set; }

        public int? PricePaid { get; set; }

        public string PayDateFrom { get; set; }

        public string PayDateTo { get; set; }

        public int? OperationType { get; set; }

        public int? PayStatus { get; set; }

        public string CitizenId { get; set; }
    }
}