using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Weather.Models;
using Weather.ViewModels;

namespace Weather.Controllers
{
    public class NewsController : Controller
    {
        private readonly int newsPerPage = 5;
        // GET: News
        [HttpGet]
        public ActionResult Index(int page = 0)
        {
            using (var context = ApplicationDbContext.Create())
            {
                var news = context.News
                    .OrderByDescending(x => x.Date)
                    .Skip(page * newsPerPage)
                    .Take(newsPerPage)
                    .ToList();
                
                var newsCount = context.News.Count();
                var userIds = news.Select(x => x.AuthorId).ToList();
                var users = context.Users.Where(u => userIds.Contains(u.Id)).ToDictionary(x => x.Id, x => x);

                var newsModels = news.Select(x => new NewsModel()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Content = x.Content.Length > 100 ? x.Content.Substring(0, 100) + "..." : x.Content,
                    Date = x.Date,
                    AuthorName = users[x.AuthorId].UserName
                }).ToList();

                var model = new NewsIndexModel()
                {
                    CurrentPage = page,
                    News = newsModels,
                    NewsPerPage = newsPerPage,
                    NewsTotalCount = newsCount
                };

                return View(model);
            }
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            using (var context = ApplicationDbContext.Create())
            {
                var news = context.News.SingleOrDefault(x => x.Id == id);

                if (news == null)
                {
                    throw new ArgumentException($"News Id: {id} does not exist");
                }

                var user = context.Users.SingleOrDefault(x => x.Id == news.AuthorId);

                if (user == null)
                {
                    throw new ArgumentException($"User Id: {news.AuthorId} does not exist");
                }

                var newsModel = new NewsModel()
                {
                    Id = news.Id,
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
        public ActionResult Create(CreateNewsModel model)
        {
            var user = System.Web.HttpContext.Current.GetOwinContext()
                .GetUserManager<ApplicationUserManager>()
                .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var news = new News()
            {
                Title = model.Title,
                Content = model.Content,
                Date = DateTime.UtcNow,
                AuthorId = user.Id
            };

            using (var context = ApplicationDbContext.Create())
            {
                context.News.Add(news);
                context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult Update(int id)
        {
            using (var context = ApplicationDbContext.Create())
            {
                var news = context.News.SingleOrDefault(x => x.Id == id);

                if (news == null)
                {
                    throw new ArgumentException($"News Id: {id} does not exist");
                }

                var newsModel = new UpdateNewsModel()
                {
                    Id = news.Id,
                    Title = news.Title,
                    Content = news.Content
                };

                return View(newsModel);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult Update(UpdateNewsModel model)
        {
            var currentUser = System.Web.HttpContext.Current.GetOwinContext()
                .GetUserManager<ApplicationUserManager>()
                .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            using (var context = ApplicationDbContext.Create())
            {
                var news = context.News.SingleOrDefault(x => x.Id == model.Id);

                if (news == null)
                {
                    throw new ArgumentException($"News Id: {model.Id} does not exist");
                }

                if (currentUser.Id != news.AuthorId && !System.Web.HttpContext.Current.User.IsInRole("Admin"))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                news.Title = model.Title;
                news.Content = model.Content;

                context.SaveChanges();

                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult Delete(int id)
        {
            var currentUser = System.Web.HttpContext.Current.GetOwinContext()
                           .GetUserManager<ApplicationUserManager>()
                           .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            using (var context = ApplicationDbContext.Create())
            {
                var news = context.News.SingleOrDefault(x => x.Id == id);

                if (news == null)
                {
                    throw new ArgumentException($"News Id: {id} does not exist");
                }

                if (currentUser.Id != news.AuthorId && !System.Web.HttpContext.Current.User.IsInRole("Admin"))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                context.News.Remove(news);
                context.SaveChanges();

                return RedirectToAction("Index");
            }
        }
    }
}