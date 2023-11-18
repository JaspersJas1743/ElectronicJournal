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
            Path = new TempFile(file: $"message_attachments_{DateTime.Now.ToString("ddMMyyyy_HHmmss")}.zip").FullName;
            FileName = System.IO.Path.GetFileName(path: Path);
            using (ZipArchive archive = ZipFile.Open(archiveFileName: Path, mode: ZipArchiveMode.Create))
            {
                foreach (string file in files)
                {
                    archive.CreateEntryFromFile(sourceFileName: file, entryName: System.IO.Path.GetFileName(file));
                }
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

            using (FileStream stream = new FileStream(path: System.IO.Path.Combine(path1: folder, path2: FileName), mode: FileMode.Create, access: FileAccess.Write))
                await stream.WriteAsync(buffer: response, offset: 0, count: response.Length, cancellationToken: cancellationToken);
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
