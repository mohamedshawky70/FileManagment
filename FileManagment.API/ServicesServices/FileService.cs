
using FileManagment.API.Data;
using FileManagment.API.Entites;

namespace FileManagment.API.ServicesServices;

public class FileService(IWebHostEnvironment webHostEnvironment, ApplicationDbContext context) : IFileService
{
	private readonly string _filePath = $"{webHostEnvironment.WebRootPath}/Uploaded";
	private readonly string _imagePath = $"{webHostEnvironment.WebRootPath}/Images";
	private readonly ApplicationDbContext _context= context;

	public async Task<Guid> UploadAsync(IFormFile file, CancellationToken cancellationToken=default)
	{
		var randomFileName = Path.GetRandomFileName();//Fake name with fake extension because if anyone reach these file on server
		var uploadFile = new UploadFile()
		{
			FileName = file.FileName,
			FileExtension = Path.GetExtension(file.FileName),
			ContentType = file.ContentType, //image/png.. or video/mp4 or any file with any extinsion
			StoredFileName = randomFileName
		};
		//Save on server
		var path = Path.Combine(_filePath, randomFileName);//string1+string2
		using var stream = File.Create(path);//حولي الباث ده لبيتس (فايل) علشان اعرف استقبل فيه الفايل
		await file.CopyToAsync(stream, cancellationToken);// إستقبل الفايل اللي جاي من اليوزر في المكان ده

		// Save on database
		await _context.AddAsync(uploadFile, cancellationToken);
		await _context.SaveChangesAsync();
		return uploadFile.Id;
	}

	public async Task UploadImageAsync(IFormFile image, CancellationToken cancellationToken = default)
	{
		var path = Path.Combine(_imagePath, image.FileName);
		using var stream = File.Create(path);
		await image.CopyToAsync(stream, cancellationToken);
	}

	public async Task<IEnumerable<Guid>> UploadManyAsync(IFormFileCollection files, CancellationToken cancellationToken = default)
	{
		List<UploadFile> UploadedFiles=[];
		foreach (var file in files)
		{
			//اتكرر حطه في بريفت ميثود
			var randomFileName = Path.GetRandomFileName();
			var uploadFile = new UploadFile()
			{
				FileName = file.FileName,
				FileExtension = Path.GetExtension(file.FileName),
				ContentType = file.ContentType, //image.ext or video.ext or any file
				StoredFileName = randomFileName
			};
			//Save on server
			var path = Path.Combine(_filePath, randomFileName);
			using var stream = File.Create(path);
			await file.CopyToAsync(stream, cancellationToken);

			//To save on database
			UploadedFiles.Add(uploadFile);
		}
		
		await _context.AddRangeAsync(UploadedFiles, cancellationToken);
		await _context.SaveChangesAsync(cancellationToken);
		return UploadedFiles.Select(x => x.Id).ToList();
	}
	public async Task<(FileStream? FileStream, string ContentType, string fileName)> Stream(Guid id, CancellationToken cancellationToken = default)
	{
		var file = await _context.Files.FindAsync(id);
		if (file is null)
			return (null, string.Empty, string.Empty);
		var path = Path.Combine(_filePath, file.StoredFileName);
		var fileSteam = File.OpenRead(path);
		return (fileSteam, file.ContentType, file.FileName);
	}
	public async Task<(byte[] fileContent, string ContentType, string fileName)> Download(Guid id, CancellationToken cancellationToken = default)
	{
		var file = await _context.Files.FindAsync(id);
		if (file is null)
			return ([], string.Empty, string.Empty);
		var path = Path.Combine(_filePath, file.StoredFileName);
		MemoryStream memoryStream = new();
		using FileStream fileStream = new(path, FileMode.Open);// اقري اللي في الباث ده
		fileStream.CopyTo(memoryStream);

		memoryStream.Position = 0;

		return (memoryStream.ToArray(), file.ContentType, file.FileName);

	}
	
}
