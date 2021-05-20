using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Model.DataProviders
{
	/// <summary>
	/// Класс для доступа к данным.
	/// Реализует точку доступа - шаблон Singleton
	/// </summary>
	public partial class NotesProvider : IAsyncProvider<CustomerNote, string>
	{
		/// <summary>
		/// Singleton pattern
		/// </summary>
		/// <returns></returns>
		public static NotesProvider GetInstance() => provider;

		private NotesProvider()
		{
			jsonSerializer = new DataContractJsonSerializer(typeof(CustomerNote));
		}

		public Task AddAsync(CustomerNote instance)
		{
			throw new NotImplementedException();
		}

		public Task Delete(CustomerNote instance)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<CustomerNote>> GetAllAsync()
		{
			throw new NotImplementedException();
		}

		public CustomerNote GetById(string id)
		{
			throw new NotImplementedException();
		}

		public Task SaveAsync()
		{
			throw new NotImplementedException();
		}
	}
}
