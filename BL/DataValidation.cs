using System;
using System.Globalization;
using System.Windows.Controls;

namespace BL
{
	public static partial class DataValidation
	{
		public static ValidationResult ValidatePhone(string phone)
		{
			if (new PhoneFormatConverter().IsPhoneValid(phone))
				return ValidationResult.ValidResult;

			return new ValidationResult(false, InvalidPhoneMessage);
		}

		public static ValidationResult ValidateName(string name)
		{
			if (IsNameValid(name))
				return ValidationResult.ValidResult;

			return new ValidationResult(false, InvalidNameMessage);
		}

		public static ValidationResult ValidateDate(string date)
		{
			var isParsed = DateTime.TryParseExact(date, "g",
											CultureInfo.CurrentCulture,
											DateTimeStyles.None,
											out var dateTime);

			if (isParsed == false)
				return new ValidationResult(false, InvalidDateFormatMessage);

			if (dateTime.Hour >= Rules.OpeningHour && dateTime.Hour < Rules.ClosingHour)
				return ValidationResult.ValidResult;

			return new ValidationResult(false, NonWorkingHourMessage);
		}
	}

	public static partial class DataValidation
	{
		private const string InvalidNameMessage = "Неизвестный формат номера! Попробуйте еще раз";

		private const string InvalidDateFormatMessage = "Неизвестный формат даты!";

		private const string NonWorkingHourMessage = "В эти часы мастерская не работает!";

		private const string InvalidPhoneMessage = "Неверный номер телефона";

		private static bool IsNameValid(string input) => !string.IsNullOrWhiteSpace(input);
	}
}
