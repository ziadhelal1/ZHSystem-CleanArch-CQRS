using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ZHSystem.Application.Common;
using ZHSystem.Application.DTOs;

namespace ZHSystem.Application.Features.Students.Queries
{
    public record GetStudentsListQuery() : IRequest<List<StudentDto>>;
    public class GetStudentListQueryHandler : IRequestHandler<GetStudentsListQuery, List<StudentDto>>
    {
        private readonly IApplicationDbContext _db;
        private readonly IMapper _mapper;

        public GetStudentListQueryHandler(IApplicationDbContext db,IMapper mapper )
        {
            _db=db;
            _mapper=mapper;
        }

        public async Task<List<StudentDto>> Handle(GetStudentsListQuery request, CancellationToken cancellationToken)
        {
            var students = await _db.Students.
                ProjectTo<StudentDto>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
            return students;
        }
    }
}
