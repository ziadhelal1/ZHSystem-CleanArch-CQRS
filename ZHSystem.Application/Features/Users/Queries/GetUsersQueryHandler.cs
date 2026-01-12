using AutoMapper;
using MediatR;
using ZHSystem.Application.Common;
using ZHSystem.Application.Common.Models;
using ZHSystem.Application.DTOs.UserMangment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ZHSystem.Application.Common.Extensions;
using ZHSystem.Application.Common.Specifications;

namespace ZHSystem.Application.Features.Users.Queries
{
    public record GetUsersQuery(
        int PageNumber = 1,
        int PageSize = 10,
        
        string? SearchUserName = null,
        string? SearchEmail = null,
        string? SearchRole = null,
        string? SortBy = null,
        bool Descending = false
    ) : IRequest<PagedResult<UserWithRolesDto>>;
    public class GetUsersQueryHandler:IRequestHandler<GetUsersQuery, PagedResult<UserWithRolesDto>>
    {
        private readonly IApplicationDbContext _db;
        private readonly IMapper _mapper;

        public GetUsersQueryHandler(
            IApplicationDbContext db,
            IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<PagedResult<UserWithRolesDto>> Handle(
            GetUsersQuery request,
            CancellationToken cancellationToken)
        {
            var spec = new UserSearchSpecification( request.SortBy, request.Descending,
                request.SearchUserName,request.SearchEmail,request.SearchRole);
            var query = _db.Users
                .AsNoTracking()
                .ApplySpecification(spec)  // Filtering + Sorting
                .ProjectTo<UserWithRolesDto>(_mapper.ConfigurationProvider);

            var pagedResult = await query.ApplyPaginationAsync(
                request.PageNumber,
                request.PageSize,
                cancellationToken
            );

            return pagedResult;
        }

    }
}
