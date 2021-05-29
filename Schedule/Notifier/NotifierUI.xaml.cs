using Controller;
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

		private readonly object locker = new object();

		private NotifierUI()
		{
			InitializeComponent();
		}

		internal static NotifierUI GetInstance() => instance;

		public void Notify(string message)
		{
			lock (locker)
			{
				try
				{
					//Получаем контекст главного потока для исполнения общращения к GUI
					Dispatcher.Invoke(() =>
					{
						messageTextBox.Opacity = 1;
						messageTextBox.Text = message;
						Task.Run(() => HideMessage());
					});
				}
				catch
				{ }
			}
		}

		private const double OpacityStep = 0.1;
		private const int OpacitySleepMs = 250;
		private const int WaitSleepMs = 1000;

		private void HideMessage()
		{
			lock (locker)
			{
				Thread.Sleep(WaitSleepMs);
				for (double i = 1; i >= 0; i -= OpacityStep)
				{
					Dispatcher?.Invoke(() => messageTextBox.Opacity = i);
					Thread.Sleep(OpacitySleepMs);
				}
			}
		}
	}
}
