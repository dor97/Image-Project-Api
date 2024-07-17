using Microsoft.AspNetCore.Identity;
using projectServer.Models.Image;

namespace projectServer.Models.Account
{
    public class UserModel : IdentityUser
    {
        /*
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        */
        public List<SampleImageModel> SampleImageModel { get; set; } = new List<SampleImageModel>();
    }
}
