using System;

namespace Controller
{
	public abstract class ControllerBase
	{
		private readonly Action<string> notifier;

		public ControllerBase(Action<string> notifier)
		{
			this.notifier = notifier;
		}

		public void Notify(string message)
		{
			notifier?.Invoke(message);
		}
	}
}
