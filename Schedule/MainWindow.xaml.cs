﻿using Controller;
using Controller.DataApis;
using Schedule.GUIs;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Schedule
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly MainController ctrl;
		readonly List<NoteItem> allViewItems = new List<NoteItem>();
		private readonly TaskScheduler uiContext;

		public MainWindow()
		{
			InitializeComponent();

			ctrl = new MainController(ToDebug);

			uiContext = TaskScheduler.FromCurrentSynchronizationContext();

			//ассинхронное получение данных и продолжение в главном потоке
			ctrl.GetDisplayedDataAsync()
				.ContinueWith(t => InitSource(t.Result),
						uiContext);
		}

		private void ToDebug(string s)
		{
			Debug.WriteLine(s);
		}

		private void InitSource(IEnumerable<INoteDisplayedData> result)
		{
			foreach (var dat in result)
			{
				var note = new NoteItem(dat);
				note.MouseDoubleClick += Note_MouseDoubleClick;

				allViewItems.Add(note);
			}

			ChangeDisplay();
		}

		/// <summary>
		/// Обновление отображения
		/// </summary>
		/// <param name="newDisplay">Коллекция записей с данными для нового отображения</param>
		private void ChangeDisplay(IEnumerable<INoteDisplayedData> newDisplay = null)
		{
			if (newDisplay is null)
			{
				notesListView.ItemsSource = allViewItems;
			}
			else
			{
				var res = allViewItems.Where(x => x.NoteData.Equals(newDisplay));

				notesListView.ItemsSource = res;
			}
		}

		private void Note_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			var note = sender as NoteItem;

			DisplayNote(note.NoteData);
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

		private void Page_ItemCreated(INoteDisplayedData obj)
		{
			//если элемент был успешно добавлен запускаем внутренний таск
			var res = ctrl.Add(obj).ContinueWith(t =>
			{
				Debug.WriteLine("в главном потоке меняет GUI");
				//в главном потоке меняет GUI
				var innerTask = new Task(() =>
				{
					NoteItem noteUI = new NoteItem(obj);
					allViewItems.Add(noteUI);
					ChangeDisplay();
					HidePage();
				},
										TaskCreationOptions.AttachedToParent);
				innerTask.Start(uiContext);

			}, TaskContinuationOptions.NotOnFaulted);
		}

		private void Page_ItemRemoved(INoteDisplayedData obj)
		{
			var itemToBeRemoved = allViewItems.FirstOrDefault(x => x.NoteData.Equals(obj));
			allViewItems.Remove(itemToBeRemoved);
			ChangeDisplay();

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
				ctrl.GetDisplayedDataAsync(first)
					.ContinueWith(t => ChangeDisplay(t.Result),
								uiContext);
			}
			else
			{
				ctrl.GetDisplayedDataAsync(first, last)
					.ContinueWith(t => ChangeDisplay(t.Result),
								uiContext);
			}

		}

		private void AddNewNoteButton_Click(object sender, RoutedEventArgs e)
		{
			var data = ctrl.AddNewNote();
			DisplayNote(data);
		}
	}
}
