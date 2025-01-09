namespace To_DoList.Domain.Interfaces;

/// <summary>
/// Базовый дженерик интерфейс с CRUD операциями
/// </summary>
public interface IBaseRepository<T>
{
    /// <summary>
    /// Добавление новой сущности в БД
    /// </summary>
    /// <param name="entity">
    /// Принимает входящий объект, который будет добавлен в БД
    /// </param>
    /// <returns></returns>
    Task CreateAsync(T entity);

    /// <summary>
    /// Метод получения всех записей из БД 
    /// </summary>
    /// <returns></returns>
    IQueryable<T> GetAll();

    /// <summary>
    /// Удаление объекта из БД
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task Remove(T entity);

    /// <summary>
    /// Обновление объекта в БД
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<T> Update(T entity);
}