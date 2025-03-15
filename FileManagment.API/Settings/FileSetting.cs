namespace FileManagment.API.Settings;

public static class FileSetting
{
	public const int FileMaxSizeInMb = 1*1024 * 1024; //KB===>B===>MB
	public static readonly string[] AllowedImagesExtension = [".jpg", ".jpeg",".png"];
	public static readonly string[] BlockSignatures = ["4D-5A", "2F-2A", "D0-CF"];
										//Signature of==>[  exe ,   msi    ,  js ]====>Executable file  بستثني هذه الفايلز للحمايه
														//لو استثنيت عن طريق الإكستنشن ممكن يغيره عادي
}
