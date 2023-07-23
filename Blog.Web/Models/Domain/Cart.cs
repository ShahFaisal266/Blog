
namespace Blog.Web.Models.Domain
{
    public class Cart
    {
        public int Id { get; set; }
            
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Amount { get; set; }
        public DateTime date { get; set; }

    }
}
