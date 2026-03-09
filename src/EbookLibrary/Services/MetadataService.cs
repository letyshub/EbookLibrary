using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml.Linq;
using UglyToad.PdfPig;

namespace EbookLibrary.Services
{
    public class MetadataService : IMetadataService
    {
        private static readonly EbookMetadata Empty = new(string.Empty, string.Empty);

        public EbookMetadata Extract(string path)
        {
            if (!File.Exists(path)) return Empty;

            var ext = Path.GetExtension(path).ToLowerInvariant();
            return ext switch
            {
                ".epub" => ExtractEpub(path),
                ".pdf"  => ExtractPdf(path),
                _       => Empty
            };
        }

        private EbookMetadata ExtractEpub(string path)
        {
            try
            {
                using var zip = ZipFile.OpenRead(path);

                // 1. Read META-INF/container.xml to find the OPF file path
                var containerEntry = zip.GetEntry("META-INF/container.xml");
                if (containerEntry == null) return Empty;

                using var containerStream = containerEntry.Open();
                var containerDoc = XDocument.Load(containerStream);
                XNamespace containerNs = "urn:oasis:names:tc:opendocument:xmlns:container";
                var opfPath = containerDoc.Descendants(containerNs + "rootfile")
                    .FirstOrDefault()?.Attribute("full-path")?.Value;

                if (opfPath == null) return Empty;

                // 2. Parse the OPF file for Dublin Core metadata
                var opfEntry = zip.GetEntry(opfPath);
                if (opfEntry == null) return Empty;

                using var opfStream = opfEntry.Open();
                var opfDoc = XDocument.Load(opfStream);
                XNamespace dc = "http://purl.org/dc/elements/1.1/";

                var title  = opfDoc.Descendants(dc + "title").FirstOrDefault()?.Value ?? string.Empty;
                var author = opfDoc.Descendants(dc + "creator").FirstOrDefault()?.Value ?? string.Empty;

                return new EbookMetadata(title.Trim(), author.Trim());
            }
            catch
            {
                return Empty;
            }
        }

        private EbookMetadata ExtractPdf(string path)
        {
            try
            {
                using var doc = PdfDocument.Open(path);
                var title  = doc.Information.Title  ?? string.Empty;
                var author = doc.Information.Author ?? string.Empty;
                return new EbookMetadata(title.Trim(), author.Trim());
            }
            catch
            {
                return Empty;
            }
        }
    }
}
