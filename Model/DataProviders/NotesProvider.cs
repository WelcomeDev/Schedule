﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
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

			var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			FullName = Path.Combine(documentsPath, FolderName, FileName);
		}

		/// <summary>
		/// Добавление объекта.
		/// Вызывается перед этим <see cref="GetAllAsync"/>, если не был вызван ранее
		/// </summary>
		/// <param name="instance"></param>
		/// <returns></returns>
		public async Task AddAsync(CustomerNote instance)
		{
			if (notes is null)
			{
				await GetAllAsync();
			}

			notes.Add(instance);
		}

		/// <summary>
		/// Удаление объекта
		/// Вызывается перед этим <see cref="GetAllAsync"/>, если не был вызван ранее
		/// </summary>
		/// <param name="instance"></param>
		/// <returns></returns>
		public async Task Delete(CustomerNote instance)
		{
			if (notes is null)
			{
				await GetAllAsync();
			}

			notes.Remove(instance);
		}

		/// <summary>
		/// Выполняется инициализация копии хранилища и возвращается коллекция
		/// </summary>
		/// <returns></returns>
		public async Task<IEnumerable<CustomerNote>> GetAllAsync()
		{
			if (notes is null)
				notes = await Task.Run(() => ReadJson());

			return notes;
		}

		/// <summary>
		/// Выполняет получение элемента по идентификатору из локальной копии
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public CustomerNote GetById(string id)
		{
			if (notes is null)
				return null;

			return notes.SingleOrDefault(x => x.Phone.Equals(id));
		}

		public async Task SaveAsync()
		{
			await Task.Run(() => WriteJson(notes));
		}
	}
}
