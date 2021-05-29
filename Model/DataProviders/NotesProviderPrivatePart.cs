using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;

namespace Model.DataProviders
{
	public partial class NotesProvider
	{
		private static readonly NotesProvider provider = new NotesProvider();

		private readonly DataContractJsonSerializer jsonSerializer;
		private readonly object locker = new object();

		private const string FileName = "LocalDb.json";
		private const string FolderName = "ScheduleDb";
		private readonly string FullName;

		private List<CustomerNote> notes;

		private List<CustomerNote> ReadJson()
		{
			using var sf = new FileStream(FullName, FileMode.OpenOrCreate, FileAccess.Read);
			var notes = new List<CustomerNote>();

			lock (locker)
			{
				try
				{
					notes = jsonSerializer.ReadObject(sf) as List<CustomerNote>;
				}
				catch
				{
					sf.Close();
					using var s = File.Create(FullName);
				}

			}

			return notes;
		}

		private void WriteJson(List<CustomerNote> notes)
		{
			using var sf = new FileStream(FullName, FileMode.OpenOrCreate, FileAccess.Write);

			lock (locker)
			{
				jsonSerializer.WriteObject(sf, notes);
			}
		}
	}
}
