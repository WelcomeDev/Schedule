using Controller.DataApis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Schedule.GUIs
{
	/// <summary>
	/// Interaction logic for NoteEditionPage.xaml
	/// </summary>
	public partial class NoteEditionPage : Page
	{
		private readonly INoteDisplayedData noteDisplayedData;

		internal event Action<INoteDisplayedData> ItemRemoved;

		internal event Action<INoteDisplayedData> ItemCreated;

		internal event Action EndOfInput;

		private readonly bool isOnCreation = false;

		public NoteEditionPage()
		{
			InitializeComponent();
		}

		public NoteEditionPage(INoteDisplayedData noteDisplayedData) : this()
		{
			isOnCreation = noteDisplayedData is null || 
				noteDisplayedData.Name is null;

			this.noteDisplayedData = noteDisplayedData;
			DataContext = noteDisplayedData;
		}

		private void OkButton_Click(object sender, RoutedEventArgs e)
		{
			if(IsValidated())
			{
				if (isOnCreation)
					ItemCreated?.Invoke(noteDisplayedData);

				EndOfInput?.Invoke();
			}
		}

		private bool IsValidated()
		{
			return true;
		}

		private void RemoveButton_Click(object sender, RoutedEventArgs e)
		{
			ItemRemoved?.Invoke(noteDisplayedData);
			EndOfInput?.Invoke();
		}

		private void HourCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}

		private void MinuteCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}
	}
}
