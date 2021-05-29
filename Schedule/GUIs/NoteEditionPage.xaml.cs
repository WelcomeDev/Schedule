using Controller;
using Controller.DataApis;
using System;
using System.Windows;
using System.Windows.Controls;
using Validator = System.Windows.Controls.Validation;

namespace Schedule.GUIs
{
	/// <summary>
	/// Interaction logic for NoteEditionPage.xaml
	/// </summary>
	public partial class NoteEditionPage : Page
	{
		private const string InvalidDataMessage = "Не все данные корректны!";

		private readonly INoteDisplayedData noteDisplayedData;

		internal event Action<INoteDisplayedData> ItemRemoved;

		internal event Action<INoteDisplayedData> ItemCreated;

		internal event Action EndOfInput;

		private readonly bool isOnCreation = false;
		private readonly NoteEditionController ctrl;

		public NoteEditionPage()
		{
			InitializeComponent();

			ctrl = new NoteEditionController(null);

			hourCB.ItemsSource = ctrl.GetHours();
			minuteCB.ItemsSource = ctrl.GetMinute();
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
			if (IsValidated())
			{
				if (isOnCreation)
					ItemCreated?.Invoke(noteDisplayedData);

				EndOfInput?.Invoke();
			}
			//TODO: выделить неверный ввод
			ctrl.Notify(InvalidDataMessage);
		}

		private bool IsValidated()
			=> !Validator.GetHasError(nameTextBox) &&
				!Validator.GetHasError(phoneTextBox) &&
				hourCB.SelectedItem != null &&
				minuteCB.SelectedItem != null &&
				dateSelector.SelectedDate != null;

		private void RemoveButton_Click(object sender, RoutedEventArgs e)
		{
			ItemRemoved?.Invoke(noteDisplayedData);
			EndOfInput?.Invoke();
		}

		private void ValidationError_Invoke(object sender, ValidationErrorEventArgs e)
		{
			ctrl.Notify(e.Error.ErrorContent.ToString());
		}
	}
}
