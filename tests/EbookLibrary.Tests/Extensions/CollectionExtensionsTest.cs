using System.Collections.Generic;
using Xunit;

namespace EbookLibrary.Tests.Extensions
{
    public class CollectionExtensionsTest
    {
        [Fact]
        public void IsNullOrEmpty_GetEmptyCollection_ReturnsTrue()
        {
            var collection = new List<string>();
            Assert.True(collection.IsNullOrEmpty());
        }

        [Fact]
        public void IsNullOrEmpty_GetNull_ReturnsTrue()
        {
            List<string> collection = null;
            Assert.True(collection.IsNullOrEmpty());
        }

        [Fact]
        public void IsNullOrEmpty_GetNotEmptyCollection_ReturnsFalse()
        {
            var collection = new List<string> { "test item" };
            Assert.False(collection.IsNullOrEmpty());
        }
    }
}
