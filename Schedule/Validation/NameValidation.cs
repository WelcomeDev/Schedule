using BL;
using System.Globalization;
using System.Windows.Controls;

namespace Schedule.Validation
{
	public class NameValidation : ValidationRule
	{
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
			=> DataValidation.ValidateName(value as string);
	}
}
