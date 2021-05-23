using System;
using System.Diagnostics.CodeAnalysis;

namespace Model
{
	[Serializable]
	public class CustomerNote : IEquatable<CustomerNote>, IComparable<CustomerNote>, INote
	{
		public string Name { get; set; }

		public string Phone { get; set; }

		public DateTime Date { get; set; }

		public CustomerNote(string name, string phone, DateTime date)
		{
			Name = name;
			Phone = phone;
			Date = date;
		}

		public override string ToString()
		{
			return $"{Name}\t{Date:g}\t{Phone}";
		}

		public override bool Equals(object obj)
		{
			if (obj is CustomerNote customerNote)
				Equals(customerNote);

			return false;
		}

		public int CompareTo([NotNull] CustomerNote other)
		{
			return Date.CompareTo(other.Date);
		}

		public bool Equals([AllowNull] CustomerNote other)
		{
			if (other is null)
				return false;

			return Phone.Equals(other.Phone);
		}
	}
}
