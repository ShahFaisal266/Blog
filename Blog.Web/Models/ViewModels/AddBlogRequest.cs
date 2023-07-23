namespace Blog.Web.Models.ViewModels
{
    public class AddBlogRequest
    {
        public string Heading { get; set; }
        public string PageTitle { get; set; }
        public string Content { get; set; }
        public string shortDescription { get; set; }
        public string FeaturedImageURl { get; set; }
        public string UrlHandle { get; set; }
        public DateTime PublishedDate { get; set; }
        public bool Visible { get; set; }
        public string Author { get; set; }
    }
}
