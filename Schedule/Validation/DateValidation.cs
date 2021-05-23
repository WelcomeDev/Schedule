using BL;
using System.Globalization;
using System.Windows.Controls;

namespace Schedule.Validation
{
	public class DateValidation : ValidationRule
	{
		public override ValidationResult Validate(object value, CultureInfo cultureInfo) 
			=> DataValidation.ValidateDate(value as string);
	}
}
