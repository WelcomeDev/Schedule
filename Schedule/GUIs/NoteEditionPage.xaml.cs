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
		public NoteEditionPage()
		{
			InitializeComponent();
		}

		public NoteEditionPage(INoteDisplayedData noteDisplayedData) : this()
		{
			DataContext = noteDisplayedData;
		}
	}
}
