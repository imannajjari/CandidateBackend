using Candidate.Data.Interfaces;
using System.Linq.Expressions;
using Candidate.Data.Context;

namespace Candidate.Data.Repositories;

public class Repository<T> : IRepository<T> where T : class, IEntity
{
    private readonly DatabaseContext _context;
    protected Repository(DatabaseContext context)
    {
        _context = context;
    }

    public virtual void Create(T entity)
    {
        entity.IsDeleted = false;
        entity.CreationDateTime = DateTime.Now;
        _context.Set<T>().Add(entity);
    }

    public virtual void Create(List<T> list)
    {
        foreach (var item in list)
        {
            item.IsDeleted = false;
            item.CreationDateTime = DateTime.Now;
            _context.Set<T>().Add(item);
        }

    }

    public virtual void Delete(int id, bool hardDelete = false)
    {
        var item = GetByID(id);
        if (hardDelete == false)
        {
            item.IsDeleted = true;
            Update(item);
        }
        else
        {
            _context.Remove(item);
        }
    }

    public virtual void Delete(List<int> list, bool hardDelete = false)
    {
        var items = GetAll(null, x => list.Contains(x.ID)).ToList();
        if (hardDelete == false)
        {
            foreach (var item in items)
            {
                item.IsDeleted = true;
            }
            Update(items);
        }
        else
        {
            _context.RemoveRange(items);
        }
    }

    public virtual void Update(T entity)
    {
        var exist = GetByID(entity.ID);
        if (exist != null)
        {
            entity.CreationDateTime = exist.CreationDateTime;
           entity.ModificationDateTime = DateTime.Now;
            _context.Entry(exist).CurrentValues.SetValues(entity);
        }
    }

    public virtual void Update(List<T> list)
    {
        var items = GetAll(null, x => list.Select(l => l.ID).Contains(x.ID)).ToList();
        foreach (var item in list)
        {
            var exist = items.FirstOrDefault(x => x.ID == item.ID);
            if (exist != null)
            {
               item.CreationDateTime = exist.CreationDateTime;
                _context.Entry(exist).CurrentValues.SetValues(item);
            }
        }
    }

    public virtual bool Exist(int id)
    {
        var query = GetAll(null, x => x.ID == id);
        return query.Any();
    }

    public virtual IQueryable<T> GetAll(bool? activate, Expression<Func<T, bool>>? where = null,
        int? pageNumber = null, int pageSize = 20, Expression<Func<T, object>>? order = null, bool desc = false)
    {

        IQueryable<T> query = _context.Set<T>();
        if (activate != null)
            query = query.Where(x => x.IsActive == activate);
        if (where != null)
            query = query.Where(where);
        if (order != null)
        {
            query = desc ? query.OrderByDescending(order) : query.OrderBy(order);
        }
        if (pageNumber != null)
            query = query.Skip((int)(pageNumber - 1) * pageSize).Take(pageSize);

        return query;

    }

    public virtual T GetByID(int id)
    {
        T entity;
        var result = GetAll(null, x => x.ID == id);
        entity = result.FirstOrDefault();
        return entity;
    }

    public virtual void Save()
    {
        _context.SaveChanges();
    }
}