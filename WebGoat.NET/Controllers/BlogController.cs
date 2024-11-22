using WebGoatCore.Models;
using WebGoatCore.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using WebGoat.NET.Models;
using WebGoat.NET.ViewModels;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace WebGoatCore.Controllers
{
    [Route("[controller]/[action]")]
    public class BlogController : Controller
    {
        private readonly BlogEntryRepository _blogEntryRepository;
        private readonly BlogResponseRepository _blogResponseRepository;

        public BlogController(BlogEntryRepository blogEntryRepository, BlogResponseRepository blogResponseRepository, NorthwindContext context)
        {
            _blogEntryRepository = blogEntryRepository;
            _blogResponseRepository = blogResponseRepository;
        }

        public IActionResult Index()
        {
            return View(_blogEntryRepository.GetTopBlogEntries());
        }

        [HttpGet("{entryId}")]
        public IActionResult Reply(int entryId)
        {
            return View(_blogEntryRepository.GetBlogEntry(entryId));
        }

        [HttpPost("{entryId}")]
        public IActionResult Reply(BlogContentViewModel request)
        {
            try
            {
                var blogContent = new BlogContents(request.Content);


                var userName = User?.Identity?.Name ?? "Anonymous";
                var response = new BlogResponse
                {
                    Author = userName,
                    Content = blogContent,
                    BlogEntryId = request.EntryId,
                    ResponseDate = DateTime.Now
                };
                _blogResponseRepository.CreateBlogResponse(response);

                return RedirectToAction("Index");
            }
            catch (ArgumentException ex)
            {

                ViewData["ErrorMessage"] = ex.Message;
                return View("Create");
            }

        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create() => View();

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(string title, BlogContentViewModel request)
        {
            var blogContent = new BlogContents(request.Content);
            var blogEntry = _blogEntryRepository.CreateBlogEntry(title, blogContent, User!.Identity!.Name!);
            return View(blogEntry);
        }



    }
}