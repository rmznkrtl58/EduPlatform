namespace EduPlatform.Web.Models.AuthViewModels
{
	public class UserViewModel
	{
		public string UserId { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; }
		public string City { get; set; }
		//GetUserProps metoduyla ihtiyacım olan verileri alacağım
		public IEnumerable<string> GetUserProps() 
		{
			yield return UserName;
			yield return Email;
			yield return City;
		}
	}
}
