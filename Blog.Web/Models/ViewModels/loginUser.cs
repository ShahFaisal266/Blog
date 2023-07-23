using System.ComponentModel;

namespace Blog.Web.Models.ViewModels
{
    public class loginUser
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        [DisplayName("Upload Profile Image")]

        public IFormFile Imgfile { get; set; }
    }
}
