using System;

namespace EbookLibrary.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToFolderName(this DateTime date)
        {
            return date.ToString("yyyy_MM_dd");
        }
    }
}
