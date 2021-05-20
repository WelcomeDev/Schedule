using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Model.DataProviders
{
	public partial class NotesProvider
	{
		private static readonly NotesProvider provider = new NotesProvider();
		private readonly DataContractJsonSerializer jsonSerializer;

		private const string FileName = "LocalDb";
		private const string FileExtention = "json";
		private const string FullName = FileName + "." + FileExtention;
	}
}
