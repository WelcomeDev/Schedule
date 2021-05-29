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

			ctrl = new NoteEditionController(Notifier.NotifierUI.GetInstance());

			nameTextBox.MaxLength = phoneTextBox.MaxLength = BL.DataValidation.MaxInputLength;

			hourCB.ItemsSource = ctrl.GetHours();
			minuteCB.ItemsSource = ctrl.GetMinutes();
		}

		public NoteEditionPage(INoteDisplayedData noteDisplayedData) : this()
		{
			isOnCreation = noteDisplayedData is null ||
				noteDisplayedData.Name is null;

			this.noteDisplayedData = noteDisplayedData;
			DataContext = noteDisplayedData;

			if (isOnCreation == false)
			{
				noteDisplayedData.Date = ctrl.TimeToCorrectFormat(noteDisplayedData.Date);
				hourCB.SelectedItem = ctrl.InitTime(noteDisplayedData.Date.Hour);
				minuteCB.SelectedItem = ctrl.InitTime(noteDisplayedData.Date.Minute);
			}
		}

		private void OkButton_Click(object sender, RoutedEventArgs e)
		{
			if (IsValidated())
			{
				if (isOnCreation)
					ItemCreated?.Invoke(noteDisplayedData);

				EndOfInput?.Invoke();
			}
			else
			{
				ctrl.Notify(InvalidDataMessage);
			}
		}

		private bool IsValidated()
			=> !string.IsNullOrWhiteSpace(nameTextBox.Text) &&
				!string.IsNullOrWhiteSpace(phoneTextBox.Text) &&
				!Validator.GetHasError(nameTextBox) &&
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

		private void HourCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var currentDate = noteDisplayedData.Date;
			var setDate = CreateDateFrom(currentDate);

			int.TryParse(hourCB.SelectedItem as string, out var hour);
			setDate = setDate.AddHours(-currentDate.Hour);  //сбрасываем в 0
			setDate = setDate.AddHours(hour);   //прибавляем выбранные

			noteDisplayedData.Date = setDate;
		}

		private DateTime CreateDateFrom(DateTime source)
		{
			return new DateTime(source.Year,
							   source.Month,
							   source.Day,
							   source.Hour,
							   source.Minute,
							   second: 0);
		}

		private void MinuteCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var currentDate = noteDisplayedData.Date;
			var setDate = CreateDateFrom(currentDate);

			int.TryParse(minuteCB.SelectedItem as string, out var minute);
			setDate = setDate.AddMinutes(-currentDate.Minute);  //сбрасываем в 0
			setDate = setDate.AddMinutes(minute);   //прибавляем выбранные

			noteDisplayedData.Date = setDate;
		}
	}
}
