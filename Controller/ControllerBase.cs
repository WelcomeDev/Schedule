using System.Threading.Tasks;

namespace Controller
{
	public abstract class ControllerBase
	{
		private readonly INotify notifier;

		public ControllerBase(INotify notifier)
		{
			this.notifier = notifier;
		}

		public void Notify(string message)
		{
			Task.Run(() => notifier?.Notify(message));
		}
	}
}
