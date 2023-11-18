namespace ElectronicJournal.API.Utilities
{
    public class FileUploader
    {
        public static async Task<string> Upload(IFormFile file)
        {
            string folder = Path.Combine(path1: "Resources", path2: "Attachments");
            DirectoryInfo directory = new DirectoryInfo(path: folder);
            if (!directory.Exists)
                directory.Create();

            string fileName = file.FileName;
            string filePath = Path.Combine(path1: folder, path2: fileName);

            using (FileStream stream = new FileStream(path: filePath, mode: FileMode.Create))
                await file.CopyToAsync(target: stream);
            return filePath;
        }
    }
}
