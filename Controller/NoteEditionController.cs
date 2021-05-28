using System;

namespace Controller
{
	public class NoteEditionController : ControllerBase
	{
		public NoteEditionController(Action<string> notifier) : base(notifier)
		{ }
	}
}
