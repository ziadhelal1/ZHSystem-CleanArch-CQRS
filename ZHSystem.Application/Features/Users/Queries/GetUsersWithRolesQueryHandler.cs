using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ZHSystem.Application.Common;
using ZHSystem.Application.DTOs.UserMangment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHSystem.Application.Features.Users.Queries
{
    public record GetUsersWithRolesQuery() : IRequest<List<UserWithRolesDto>>;
    public class GetUsersWithRolesQueryHandler:IRequestHandler<GetUsersWithRolesQuery,List<UserWithRolesDto>>
    {
        private readonly IApplicationDbContext _db;
        private readonly IMapper _mapper;

        public GetUsersWithRolesQueryHandler(IApplicationDbContext db,IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        
        public async Task<List<UserWithRolesDto>> Handle(GetUsersWithRolesQuery request, CancellationToken cancellationToken)
        {
            return await _db.Users
                .AsNoTracking()
                .ProjectTo<UserWithRolesDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
           

        }
    }
}
