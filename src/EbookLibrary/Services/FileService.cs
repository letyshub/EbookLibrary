using System;
using System.Diagnostics;
using System.IO;

namespace EbookLibrary
{
    public class FileService : IFileService
    {
        public void DeleteFile(string path)
        {
            File.Delete(path);
        }

        public void OpenFile(string path)
        {
            if (!string.IsNullOrWhiteSpace(path))
            {
                var p = new Process();
                p.StartInfo = new ProcessStartInfo(path)
                {
                    UseShellExecute = true
                };
                p.Start();
            }
        }

        public string SaveFile(string path)
        {
            var fi = new FileInfo(path);
            var filename = Guid.NewGuid().ToString();
            var folder = $"{Properties.EbookLibrary.Default.EbooksFolder}\\{DateTime.Now.ToString("YYYY_MM_DD")}";
            var storedEbookPath = $"{folder}\\{filename}.{fi.Extension}";

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            File.Copy(path, storedEbookPath, true);

            return storedEbookPath;
        }
    }
}
