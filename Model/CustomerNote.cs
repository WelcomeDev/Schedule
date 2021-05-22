using System;
using System.Diagnostics.CodeAnalysis;

namespace Model
{
	[Serializable]
	public class CustomerNote : IEquatable<CustomerNote>, IComparable<CustomerNote>
	{
		public string Name { get; set; }

		public string Phone { get; set; }

		public DateTime Date { get; set; }

		public CustomerNote(string name, string phone, DateTime date)
		{

		}

		public override string ToString()
		{
			return base.ToString();
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public int CompareTo([AllowNull] CustomerNote other)
		{
			throw new NotImplementedException();
		}

		public bool Equals([AllowNull] CustomerNote other)
		{
			throw new NotImplementedException();
		}
	}
}
