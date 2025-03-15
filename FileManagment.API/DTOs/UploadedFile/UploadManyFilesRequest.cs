namespace FileManagment.API.DTOs.UploadedFile;

public record UploadManyFilesRequest
(
	IFormFileCollection Files
);
