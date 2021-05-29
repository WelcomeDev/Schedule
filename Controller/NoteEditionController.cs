using System;
using System.Collections;

namespace Controller
{
	public class NoteEditionController : ControllerBase
	{
		public NoteEditionController(Action<string> notifier) : base(notifier)
		{ }

		public IEnumerable GetHours()
		{
			throw new NotImplementedException();
		}

		public IEnumerable GetMinute()
		{
			throw new NotImplementedException();
		}
	}
}
