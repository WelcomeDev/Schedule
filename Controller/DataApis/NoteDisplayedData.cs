using Model;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Controller.DataApis
{
	public class NoteDisplayedData : INoteDisplayedData, IEquatable<NoteDisplayedData>
	{
		private readonly INote note;

		public event PropertyChangedEventHandler PropertyChanged;

		public string Name 
		{ 
			get => note.Name; 
			set 
			{
				note.Name = value; 
				NotifyPropertyChanged("Name"); 
			} 
		}

		public string Phone 
		{ 
			get => note.Phone;
			set
			{
				note.Phone = value;
				NotifyPropertyChanged("Phone");
			}
		}

		public DateTime Date 
		{ 
			get => note.Date;
			set
			{
				note.Date = value;
				NotifyPropertyChanged("Date");
			}
		}

		internal NoteDisplayedData(INote note)
		{
			this.note = note;
		}

		private void NotifyPropertyChanged(string propName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
		}

		public override string ToString()
		{
			return note.ToString();
		}

		public override bool Equals(object obj)
		{
			if (obj is INoteDisplayedData displayedData)
				return Equals(displayedData);

			return false;
		}

		public bool Equals([AllowNull] INoteDisplayedData other)
		{
			if (other is null)
				return false;

			return Name.Equals(other.Name) &&
				Phone.Equals(other.Phone) &&
				Date.Equals(other.Date);
		}

		public bool Equals([AllowNull] NoteDisplayedData other)
		{
			return Equals(other as INoteDisplayedData);
		}
	}
}
