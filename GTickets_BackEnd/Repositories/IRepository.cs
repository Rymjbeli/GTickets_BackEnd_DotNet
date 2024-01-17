using System;
using System.Collections.Generic;

namespace GTickets_BackEnd.Repositories
{
    public interface IRepository<T,id> where T : class
    {
        ICollection<T> GetAll();
        T GetById(id id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
