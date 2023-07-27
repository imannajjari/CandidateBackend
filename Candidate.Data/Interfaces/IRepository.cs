using System.Linq.Expressions;

namespace Candidate.Data.Interfaces;

public interface IRepository<T> where T : class, IEntity
{
    void Create(T entity);
    void Create(List<T> list);
    void Update(T entity);
    void Update(List<T> list);
    IQueryable<T> GetAll(bool? activate, Expression<Func<T, bool>>? where = null, int? pageNumber = null, int pageSize = 20, Expression<Func<T, object>>? order = null, bool desc = false);
    T? GetByID(int id);
    void Delete(int id, bool hardDelete = false);
    void Delete(List<int> list, bool hardDelete = false);
    bool Exist(int id);
    void Save();
}