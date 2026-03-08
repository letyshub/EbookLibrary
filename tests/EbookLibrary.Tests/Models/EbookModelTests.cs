using System.Collections.Generic;
using EbookLibrary.Models;
using NSubstitute;
using Xunit;

namespace EbookLibrary.Tests.Models
{
    public class EbookModelTests
    {
        private readonly IFileService fileService = Substitute.For<IFileService>();
        private readonly IDatabaseService dbService = Substitute.For<IDatabaseService>();
        private readonly EbookModel model;

        public EbookModelTests()
        {
            model = new EbookModel(fileService, dbService);
        }

        [Fact]
        public void ToggleReadStatus_MarkAsRead_CallsUpdateReadStatusWithTrue()
        {
            // Act
            model.ToggleReadStatus(1, true);

            // Assert
            dbService.Received(1).UpdateReadStatus(1, true);
        }

        [Fact]
        public void ToggleReadStatus_MarkAsUnread_CallsUpdateReadStatusWithFalse()
        {
            // Act
            model.ToggleReadStatus(5, false);

            // Assert
            dbService.Received(1).UpdateReadStatus(5, false);
        }

        [Fact]
        public void GetByReadStatus_ReturnsResultFromDb()
        {
            // Arrange
            var expected = new List<Ebook> { new Ebook { Id = 1, IsRead = true } };
            dbService.GetByReadStatus(true).Returns(expected);

            // Act
            var result = model.GetByReadStatus(true);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Get_WithReadFilter_PassesFilterToDb()
        {
            // Arrange
            dbService.GetByQuery("test", true).Returns(new List<Ebook>());

            // Act
            model.Get("test", true);

            // Assert
            dbService.Received(1).GetByQuery("test", true);
        }

        [Fact]
        public void Get_WithoutReadFilter_PassesNullToDb()
        {
            // Arrange
            dbService.GetByQuery("test", null).Returns(new List<Ebook>());

            // Act
            model.Get("test");

            // Assert
            dbService.Received(1).GetByQuery("test", null);
        }

        [Fact]
        public void Update_PassesIsReadAndPriorityToDb()
        {
            // Act
            model.Update(1, "Title", "Author", new List<string> { "tag" }, true, 3);

            // Assert
            dbService.Received(1).Update(1, "Title", "Author", Arg.Any<List<string>>(), true, 3);
        }
    }
}
