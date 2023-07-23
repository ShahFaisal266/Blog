using Blog.Web.Models.Domain;

namespace Blog.Web.Models.ViewModels
{
    public class BookedItemReq
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ItemName { get; set; }
        public loginUser users { get; set; }
        public AddBlogRequest addBlogRequests { get; set; }
    }
}
