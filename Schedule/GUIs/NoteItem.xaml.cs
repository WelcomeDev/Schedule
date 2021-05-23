using Controller.DataApis;
using System.Windows.Controls;

namespace Schedule.GUIs
{
	/// <summary>
	/// Interaction logic for NoteItem.xaml
	/// </summary>
	public partial class NoteItem : UserControl
	{
		public INoteDisplayedData NoteData { get; }

		public NoteItem()
		{
			InitializeComponent();
		}

		public NoteItem(INoteDisplayedData noteDisplayedData) : this()
		{
			DataContext = NoteData = noteDisplayedData;
		}
	}
}
