using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace EbookLibrary
{
    public class RequiredRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
            {
                return new ValidationResult(false, "Required");
            }
            var text = this.GetBoundValue(value) as string;

            return string.IsNullOrWhiteSpace(text) ? new ValidationResult(false, "Required") : new ValidationResult(true, null);
        }

        private object GetBoundValue(object value)
        {
            if (value is BindingExpression)
            {
                BindingExpression binding = (BindingExpression)value;

                object dataItem = binding.DataItem;
                string propertyName = binding.ParentBinding.Path.Path;
                object propertyValue = dataItem.GetType().GetProperty(propertyName).GetValue(dataItem, null);

                return propertyValue;
            }
            else
            {
                return value;
            }
        }
    }
}
