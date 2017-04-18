using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Weather.Models
{
    public class NewsModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string AuthorName { get; set; }
        public DateTime Date { get; set; }
    }
}