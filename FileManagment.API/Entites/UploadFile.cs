namespace FileManagment.API.Entites;

public class UploadFile
{
	public Guid Id { get; set; } = Guid.CreateVersion7();
	public string FileName { get; set; } = default!;
	public string StoredFileName { get; set; } = default!;
	public string ContentType { get; set; } = default!;
	public string FileExtension { get; set; } = default!;
}
