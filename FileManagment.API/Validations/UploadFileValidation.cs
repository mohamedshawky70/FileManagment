using FileManagment.API.DTOs.UploadedFile;
using FileManagment.API.Entites;
using FileManagment.API.Settings;
using FluentValidation;

namespace FileManagment.API.Validations;

public class UploadFileValidation:AbstractValidator<UploadFileRequest>
{
	public UploadFileValidation()
	{
	/*	RuleFor(x => x.File)
			.Must((request, context) => request.File.Length <= FileSetting.FileMaxSizeInMb)
			.WithMessage($"Max file size {FileSetting.FileMaxSizeInMb / 1024 / 1024} MB")
			.When(x => x.File is not null);//علشان متبلعش نل في اللينث*/

		RuleFor(x => x.File)
			.Must((request, context) =>
			{
				BinaryReader binary = new(request.File.OpenReadStream());//حول الفايل لبيناري
				var bytes = binary.ReadBytes(2);// اقري اول اتنين بينري

				// Convert the bytes to a hexadecimal string for comparison
				var fileSequenceHex = BitConverter.ToString(bytes);//2F-2A ....

				// Check if the signature matches any blocked signatures
				foreach (var signature in FileSetting.BlockSignatures)
				{
					if (signature.Equals(fileSequenceHex, StringComparison.OrdinalIgnoreCase))
						return false; // File signature is blocked
				}
				return true;
			})
			.WithMessage("Not allowed file content")
			.When(x => x.File is not null);
	}
}
