using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Schedule.Notifier
{
	/// <summary>
	/// Оповещение пользователя через GUI
	/// </summary>
	public partial class NotifierUI : UserControl, INotify
	{
		private static readonly NotifierUI instance = new NotifierUI();

		private NotifierUI()
		{
			InitializeComponent();
		}

		internal static NotifierUI GetInstance() => instance;

		public void Notify(string message)
		{
			messageTextBox.Opacity = 1;
			messageTextBox.Text = message;
			Task.Run(() => HideMessage());
		}

		private const double OpacityStep = 0.1;
		private const int OpacitySleepMs = 50;
		private const int WaitSleepMs = 100;

		private void HideMessage()
		{
			Thread.Sleep(WaitSleepMs);
			for (int i = 1; i >= 0; i--)
			{
				Dispatcher?.Invoke(() => messageTextBox.Opacity = i * OpacityStep);
				Thread.Sleep(OpacitySleepMs);
			}
		}
	}
}
