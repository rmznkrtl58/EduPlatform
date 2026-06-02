using EduPlatform.Web.Models.DiscountViewModels;
using FluentValidation;
namespace EduPlatform.Web.Validators
{
	public class DiscountApplyRequestValidators : AbstractValidator<DiscountApplyRequest>
	{
		public DiscountApplyRequestValidators()
		{
			RuleFor(x => x.Code).NotEmpty().WithMessage("indirim kupon alanı boş olamaz");
		}
	}
}
