using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZHSystem.Application.Common.Specifications;

namespace ZHSystem.Application.Common.Extensions
{
    public static class SpecificationExtensions
    {
        public static IQueryable<T> ApplySpecification<T>(
            this IQueryable<T> query,
            ISpecification<T> specification)
        {
            return specification.Apply(query);
        }

    }
}
