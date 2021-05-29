using System.Collections.Generic;
using System.Threading.Tasks;

namespace Model.DataProviders
{
	/// <summary>
	/// Интерфейс для провайдера данных типа <typeparamref name="TType"/>
	/// </summary>
	/// <typeparam name="TType">тип, с которым работает провайдер</typeparam>
	/// <typeparam name="TId">тип идентификатора</typeparam>
	public interface IAsyncProvider<TType, TId>
					where TType : class
	{
		/// <summary>
		/// Ассинхронное добавление
		/// </summary>
		/// <param name="instance">объект для добавления</param>
		/// <returns>Задача асинхронной операции</returns>
		Task AddAsync(TType instance);

		/// <summary>
		/// Удаление
		/// </summary>
		/// <param name="instance">объект для удаления</param>
		void Delete(TType instance);

		/// <summary>
		/// Получить по id
		/// </summary>
		/// <param name="id">id</param>
		/// <returns>искомый объект или null</returns>
		TType GetById(TId id);

		/// <summary>
		/// Ассинхронное получение всей коллекции в формате списка
		/// </summary>
		/// <returns>Задача асинхронной операции с результатом</returns>
		Task<IEnumerable<TType>> GetAllAsync();

		/// <summary>
		/// Сохранение изменений
		/// </summary>
		Task SaveAsync();
	}
}
