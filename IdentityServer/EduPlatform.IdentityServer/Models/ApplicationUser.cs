using Microsoft.AspNetCore.Identity;

namespace EduPlatform.IdentityServer.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
		public string Name { get; set; }
		public string Surname { get; set; }
		public string City { get; set; }
		
	}
}
