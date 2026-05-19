using System.ComponentModel.DataAnnotations;

namespace EduPlatform.Web.Models.AuthViewModels
{
	public class SignInRequestModel
	{
		[Required]
		[Display(Name ="Email Adresiniz")]
		public string Email { get; set; }
		[Required]
		[Display(Name = "Şifreniz")]
		public string Password { get; set; }
		[Display(Name = "Beni Hatırla")]
		public bool IsRemember { get; set; }
	}
}
