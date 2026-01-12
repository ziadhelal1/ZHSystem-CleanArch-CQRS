using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ZHSystem.Application.Common.Specifications
{
    public class BaseSpecification<T> : ISpecification<T> where T : class
    {
        public Expression<Func<T, bool>>? Criteria { get; protected set; }

        public List<string> Includes { get; } = new();
       // public List<Expression<Func<T, object>>> Includes { get; } = new();
        public Expression<Func<T, object>>? OrderBy { get; protected set; }
        public Expression<Func<T, object>>? OrderByDescending { get; protected set; }
        public IQueryable<T> Apply(IQueryable<T> query)
        {
            if (Criteria != null)
                query = query.Where(Criteria);

            // Apply includes
            foreach (var include in Includes)
            {
                query = query.Include(include);
            }
            if (OrderBy != null)
                query = query.OrderBy(OrderBy);
            else if (OrderByDescending != null)
                query = query.OrderByDescending(OrderByDescending);
            

            return query;
        }
    }
}
