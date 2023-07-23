using Blog.Web.Data;
using Blog.Web.Models.Domain;
using Blog.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Controllers
{
    public class AdminBlogController : Controller
    {
        private readonly BlogDbContext blogDbContext;

        public AdminBlogController(BlogDbContext blogDbContext)
        {
            this.blogDbContext = blogDbContext;
        }
        [HttpGet]
        public async Task<IActionResult> ViewBlog()
        {
            var blogPosts = await blogDbContext.BlogPosts.ToListAsync();
            return View(blogPosts);
        }
        public async Task<IActionResult> Blogs()
        {
            var blogPost=await blogDbContext.BlogPosts.ToListAsync();
            return View(blogPost);
        }
        [HttpPost]
        [Route("blogs/{id:guid}")]
        public async Task<IActionResult> GetOneBlog([FromRoute] Guid id)
        {
            var blogPost = await blogDbContext.BlogPosts.FindAsync(id);
            if (blogPost != null)
            {
                return View(blogPost);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("DeleteView/{id:guid}")]
        public async Task<IActionResult> DeleteView(Guid id)
        {
            var blog = await blogDbContext.BlogPosts.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }

            blogDbContext.BlogPosts.Remove(blog);
            await blogDbContext.SaveChangesAsync();
            return RedirectToAction("ViewBlog");
        }

        [HttpGet]
        public IActionResult Add() //Response return view        {
        {

            return View();
        }

        [HttpPost]
        public IActionResult Add(AddBlogRequest addBlogRequest)
        {
            //one way
            //  var name = Request.Form["name"];
            // var displayName = Request.Form["displayName"];//form

            //second way
            var blog = new BlogPost
            {
               Author=addBlogRequest.Author,
               Heading=addBlogRequest.Heading,
               Content=addBlogRequest.Content,
               UrlHandle=addBlogRequest.UrlHandle,
               FeaturedImageURl=addBlogRequest.FeaturedImageURl,
               shortDescription=addBlogRequest.shortDescription,
               Visible=addBlogRequest.Visible,
               PageTitle=addBlogRequest.PageTitle,
               PublishedDate=addBlogRequest.PublishedDate

            };

            blogDbContext.BlogPosts.Add(blog);
            blogDbContext.SaveChanges();
            return View("Add");

        }
        
    }
}
