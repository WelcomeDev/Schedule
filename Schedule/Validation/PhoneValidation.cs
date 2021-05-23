using BL;
using System.Globalization;
using System.Windows.Controls;

namespace Schedule.Validation
{
	public class PhoneValidation : ValidationRule
	{
		public override ValidationResult Validate(object value, CultureInfo cultureInfo) 
			=> DataValidation.ValidatePhone(value as string);
	}
}
