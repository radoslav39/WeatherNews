using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Weather.ViewModels
{
    public class NewsIndexModel
    {
        public int CurrentPage { get; set; }
        public int NewsPerPage { get; set; }
        public int NewsTotalCount { get; set; }
        public List<NewsModel> News { get; set; }
    }
}