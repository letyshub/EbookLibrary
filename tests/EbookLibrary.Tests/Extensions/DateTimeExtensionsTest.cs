using EbookLibrary.Extensions;
using System;
using Xunit;

namespace EbookLibrary.Tests.Extensions
{
    public class DateTimeExtensionsTest
    {
        [Fact]
        public void ToFolderName_GetsDate_ReturnFolderName()
        {
            var dt = new DateTime(2020, 6, 11);
            var actual = dt.ToFolderName();
            var expected = "2020_06_11";
            Assert.Equal(expected, actual);
        }
    }
}
