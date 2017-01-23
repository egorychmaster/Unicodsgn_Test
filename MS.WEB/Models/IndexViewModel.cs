using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MS.WEB.Models
{
    public class IndexViewModel
    {
        public IEnumerable<QueueView> Queues { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}