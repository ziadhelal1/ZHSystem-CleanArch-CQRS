using Microsoft.EntityFrameworkCore;
using ZHSystem.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHSystem.Application.Common.Extensions
{
    public static class PaginationExtensions
    {
        public static async Task<PagedResult<T>> ApplyPaginationAsync<T>(
            this IQueryable<T> query,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            const int MaxPageSize = 50;
            pageSize = pageSize < 1 ? 10 :
                pageSize > MaxPageSize ? MaxPageSize :
                pageSize;

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<T>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };

        }

    }
}
