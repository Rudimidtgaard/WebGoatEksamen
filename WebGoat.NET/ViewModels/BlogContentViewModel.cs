using WebGoat.NET.Models;
using WebGoatCore.Models;

namespace WebGoat.NET.ViewModels
{
    public class BlogContentViewModel
    {
        public string blogContents {  get; set; }


        public BlogResponse ToModel()
        {
            return new BlogResponse(
                new BlogContents(blogContents)
                );
        }
    }
}
