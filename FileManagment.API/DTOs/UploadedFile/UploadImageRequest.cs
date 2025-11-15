namespace FileManagment.API.DTOs.UploadedFile;

public record UploadImageRequest
(
	public IFormFile Image { get;set;}
);
