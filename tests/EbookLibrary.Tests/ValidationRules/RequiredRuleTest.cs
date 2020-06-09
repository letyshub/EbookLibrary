using System.Globalization;
using Xunit;

namespace EbookLibrary.Tests.ValidationRules
{
    public class RequiredRuleTest
    {
        private RequiredRule rule = new RequiredRule();
        private CultureInfo ci = new CultureInfo("pl-PL", false);

        [Theory]
        [InlineData("test", true, null)]
        [InlineData("", false, "Required")]
        [InlineData(null, false, "Required")]
        public void Validate_GetsText_ReturnExpected(string text, bool validationResult, string validationMessage)
        {
            var actual = rule.Validate(text, ci);
            Assert.Equal(validationResult, actual.IsValid);
            Assert.Equal(validationMessage, actual.ErrorContent);
        }
    }
}
