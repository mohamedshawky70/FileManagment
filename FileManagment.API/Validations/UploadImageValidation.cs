using FileManagment.API.DTOs.UploadedFile;
using FileManagment.API.Settings;
using FluentValidation;

namespace FileManagment.API.Validations;

public class UploadImageValidation:AbstractValidator<UploadImageRequest>
{
	public UploadImageValidation()
	{
		RuleFor(x=>x.Image)
			.Must(x => x.Length<= FileSetting.FileMaxSizeInMb)
			.WithMessage($"Max image size {FileSetting.FileMaxSizeInMb / 1024 / 1024} MB")
			.When(x => x.Image is not null);//علشان متبلعش نل في اللينث
		// لو غير الاكتنشن بتاع ملف الجافاسكربت هيقبله عادي علشان كده الافضل تعتمد علي السيجنتشر
		RuleFor(x => x.Image)
			.Must(x =>
			{
				var ExtensionImage = Path.GetExtension(x.FileName.ToLower());//ممكن يجولك كابتل
				return FileSetting.AllowedImagesExtension.Contains(ExtensionImage); // true or false
			})
			.WithMessage("Not allowed image extension")
			.When(x => x.Image is not null);
	}
}
