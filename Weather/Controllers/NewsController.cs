using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Weather.Models;

namespace Weather.Controllers
{
    public class NewsController : Controller
    {
        // GET: News
        [HttpGet]
        public ActionResult Index()
        {

            return View();
        }

        [HttpGet]
        public ActionResult Details(int newsId)
        {
            using (var context = ApplicationDbContext.Create())
            {
                var news = context.News.SingleOrDefault(x => x.Id == newsId);

                if (news == null)
                {
                    throw new ArgumentException($"News Id: {newsId} does not exist");
                }

                var user = context.Users.SingleOrDefault(x => x.Id == news.AuthorId);

                if (user == null)
                {
                    throw new ArgumentException($"User Id: {news.AuthorId} does not exist");
                }

                var newsModel = new NewsModel()
                {
                    Title = news.Title,
                    Content = news.Content,
                    Date = news.Date,
                    AuthorName = user.UserName
                };

                return View(newsModel);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Update()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Delete()
        {
            return View();
        }
    }
}