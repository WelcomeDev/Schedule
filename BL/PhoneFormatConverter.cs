using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BL
{
	public partial class PhoneFormatConverter
	{
		/// <summary>
		/// Общий формат приложения
		/// </summary>
		/// <param name="phone"></param>
		/// <returns></returns>
		public string ToAppFormat(string phone)
		{
			if (IsPhoneValid(phone))
				return ConvertToFormat(phone);

			return null;
		}

		//SOLVE: добавить тестов
		public bool IsPhoneValid(string phone) => Regex.IsMatch(phone, PhonePattern);
	}

	public partial class PhoneFormatConverter
	{
		private const string PhonePattern = @"^(8[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$";
		private const int ExtraCharsAmount = 4;

		private string ConvertToFormat(string phone)
		{
			var extractNumbers = phone.Where(x => x >= '0' && x <= '9').ToList();

			StringBuilder s = new StringBuilder(extractNumbers.Count + ExtraCharsAmount);
			for (int i = 0; i < extractNumbers.Count; i++)
			{
				if (i == 0)
					s.Append('(');
				else if (i == 3)
					s.Append(')');
				else if (i == 7 || i==9)
					s.Append('-');

				s.Append(extractNumbers[i]);
			}

			return s.ToString();
		}
	}
}
