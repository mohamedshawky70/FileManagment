namespace FileManagment.API.ServicesServices;

public interface IFileService
{
	Task<Guid> UploadAsync(IFormFile file,CancellationToken cancellationToken = default);
	Task<IEnumerable<Guid>> UploadManyAsync(IFormFileCollection files,CancellationToken cancellationToken = default);
	Task UploadImageAsync(IFormFile image,CancellationToken cancellationToken = default);
	Task<(byte[]fileContent,string ContentType, string fileName)> Download(Guid id,CancellationToken cancellationToken = default);
	Task<(FileStream? FileStream,string ContentType, string fileName)> Stream(Guid id,CancellationToken cancellationToken = default);
}
