using FileManagment.API.DTOs.UploadedFile;
using FileManagment.API.Entites;
using FileManagment.API.Settings;
using FluentValidation;

namespace FileManagment.API.Validations;

public class UploadManyFilesValidation : AbstractValidator<UploadManyFilesRequest>
{
	public UploadManyFilesValidation()
	{
		RuleForEach(x => x.Files)
			//.SetValidator(new FileSizeValidator)ممكن الجزء ده تنقله في فاليديتور لوحده علشان تستخدمه هنا وهناك وكذلك الجزء اللي تحت
			.Must((request, file) => file.Length <= FileSetting.FileMaxSizeInMb)
			.WithMessage($"Max file size {FileSetting.FileMaxSizeInMb / 1024 / 1024} MB")
			.When(x => x.Files is not null);

		RuleForEach(x => x.Files)
			.Must((request, file) =>
			{
				using BinaryReader binary = new(file.OpenReadStream());
				var bytes = binary.ReadBytes(2);
				var fileSequenceHex = BitConverter.ToString(bytes);

				foreach (var signature in FileSetting.BlockSignatures)
				{
					if (signature.Equals(fileSequenceHex, StringComparison.OrdinalIgnoreCase))
						return false;
				}
				return true;
			})
			.WithMessage("Not allowed file content")
			.When(x => x.Files is not null);
	}
}
