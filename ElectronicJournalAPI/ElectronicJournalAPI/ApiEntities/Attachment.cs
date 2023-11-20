using ElectronicJournalAPI.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

namespace ElectronicJournalAPI.ApiEntities
{
    public class Attachment
    {
        public Attachment() { }

        public Attachment(string file)
        {
            Path = file;
            FileName = System.IO.Path.GetFileName(file);
        }

        public Attachment(IEnumerable<string> files)
        {
            TempFile f = new TempFile(file: $"message_attachments_{DateTime.Now.ToString("ddMMyyyy_HHmmss")}.zip");
            Path = f.FullName;
            FileName = f.Name;
            using (ZipArchive archive = ZipFile.Open(archiveFileName: Path, mode: ZipArchiveMode.Create))
            {
                foreach (string file in files)
                    archive.CreateEntryFromFile(sourceFileName: file, entryName: System.IO.Path.GetFileName(file));
            }
        }

        public class UploadAttachmentResponse
        {
            public int Id { get; set; }
        }

        public int Id { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }

        public async Task Download(string folder, CancellationToken cancellationToken = default)
        {
            byte[] response = await ApiClient.GetBytesAsync(
                apiMethod: "Attachments/DownloadAttachment",
                argQuery: new Dictionary<string, string>
                {
                    ["Id"] = Id.ToString()
                });

            string newPath = System.IO.Path.Combine(path1: folder, path2: FileName);
            using (FileStream stream = new FileStream(path: newPath, mode: FileMode.Create, access: FileAccess.Write))
                await stream.WriteAsync(buffer: response, offset: 0, count: response.Length, cancellationToken: cancellationToken);
            Path = newPath;
        }

        public async Task<UploadAttachmentResponse> Upload(CancellationToken cancellationToken = default)
        {
            ApiClient.ContentType = "application/zip";
            return await ApiClient.PostFileAsync<UploadAttachmentResponse>(
                apiMethod: "Attachments/UploadAttachment",
                path: Path,
                cancellationToken: cancellationToken
            );
        }
    }
}
