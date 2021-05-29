﻿using Controller;
using Controller.DataApis;
using Schedule.GUIs;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Schedule
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly MainController ctrl;
		private readonly TaskScheduler uiContext;

		private readonly MainWindowData mainWinData;

		public MainWindow()
		{
			InitializeComponent();

			uiContext = TaskScheduler.FromCurrentSynchronizationContext();

			ctrl = new MainController(ToDebug);
			mainWinData = new MainWindowData();
			DataContext = mainWinData;

			mainWinData.SetDate(DateTime.Today);
			//ассинхронное получение данных и продолжение в главном потоке
			ctrl.GetDisplayedDataAsync()
				.ContinueWith(t => mainWinData.InitSource(t.Result),
						uiContext);
		}

		private void ToDebug(string s)
		{
			Debug.WriteLine(s);
		}

		/// <summary>
		/// Отображает выбранную запись в <see cref="NoteEditionPage"/>
		/// </summary>
		/// <param name="note"></param>
		private void DisplayNote(INoteDisplayedData note)
		{
			var page = new NoteEditionPage(note);
			page.ItemRemoved += Page_ItemRemoved;
			page.EndOfInput += HidePage;
			page.ItemCreated += Page_ItemCreated;

			AddNoteFrame.Navigate(page);
		}

		private void HidePage()
		{
			AddNoteFrame.Navigate(null);
		}

		/// <summary>
		/// Обработка события со страницы - создание объекта
		/// </summary>
		/// <param name="obj"></param>
		private void Page_ItemCreated(INoteDisplayedData obj)
		{
			//если элемент был успешно добавлен запускаем внутренний таск
			var res = ctrl.Add(obj).ContinueWith(t =>
			{
				Debug.WriteLine("в главном потоке меняет GUI");
				//в главном потоке меняет GUI
				var innerTask = new Task(() =>
							{
								mainWinData.Add(obj);
								HidePage();
							},
								TaskCreationOptions.AttachedToParent);
				innerTask.Start(uiContext);

			}, TaskContinuationOptions.NotOnFaulted);
		}

		/// <summary>
		/// Обработка события со страницы - удаление объекта
		/// </summary>
		/// <param name="obj"></param>
		private void Page_ItemRemoved(INoteDisplayedData obj)
		{
			mainWinData.Remove(obj);
			ctrl.Remove(obj);
		}

		private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
		{
			var cal = sender as Calendar;

			var dates = cal.SelectedDates;
			var first = dates.First();
			var last = dates.Last();

			if (first.Equals(last))
			{
				mainWinData.SetDate(first);
				ctrl.GetDisplayedDataAsync(first)
					.ContinueWith(t => mainWinData.DisplayDates(t.Result),
								uiContext);
			}
			else
			{
				mainWinData.SetDates(first, last);
				ctrl.GetDisplayedDataAsync(first, last)
					.ContinueWith(t => mainWinData.DisplayDates(t.Result),
								uiContext);
			}

		}

		private void AddNewNoteButton_Click(object sender, RoutedEventArgs e)
		{
			var data = ctrl.AddNewNote();
			DisplayNote(data);
		}

		/// <summary>
		/// Обработка события выбора элемента в листе
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void NotesListView_ItemSelected(object sender, RoutedEventArgs e)
		{
			if (sender is ListView lv)
			{
				var li = lv.SelectedItem as NoteItem;
				DisplayNote(li?.NoteData);
			}
		}

		private void MainGrid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			mainGrid.Focus();
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			ctrl.Save();
		}
	}
}
