using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace ASP2236903.Models
{
    public class BaseModelo
    {
        public int ActualPage { get; set; }
        public int Total { get; set; }
        public int RecordsPage { get; set; }
        public RouteValueDictionary valueQueryString { get; set; }
    }
}