using EduPlatform.Web.Models.CatalogViewModels;
using FluentValidation;

namespace EduPlatform.Web.Validators
{
	public class UpdateCourseRequestValidators:AbstractValidator<UpdateCourseRequest>
	{
		public UpdateCourseRequestValidators()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage("isim alanı boş olamaz");
			RuleFor(x => x.Description).NotEmpty().WithMessage("açıklama alanı boş olamaz");
			RuleFor(x => x.Feature.Duration).InclusiveBetween(1, int.MaxValue).WithMessage("süre alanı boş olamaz");
			RuleFor(x => x.Price).NotEmpty().WithMessage("fiyat alanı boş olamaz").ScalePrecision(2, 6).WithMessage("hatalı para formatı");
		}
	}
}
