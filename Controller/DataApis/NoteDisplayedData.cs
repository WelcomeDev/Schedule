using Model;
using System;
using System.ComponentModel;

namespace Controller.DataApis
{
	public class NoteDisplayedData : INoteDisplayedData, INotifyPropertyChanged
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
	}
}
