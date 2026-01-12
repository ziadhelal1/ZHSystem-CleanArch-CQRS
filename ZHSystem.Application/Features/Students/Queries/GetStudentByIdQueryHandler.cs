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
    public record GetStudentByIdQuery(int Id ):IRequest<StudentDto?>;
    public class GetStudentByIdQueryHandler : IRequestHandler<GetStudentByIdQuery, StudentDto>
    {
        private readonly IApplicationDbContext _db;
        private readonly IMapper _mapper;
        public GetStudentByIdQueryHandler(IApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<StudentDto> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
        {
            var student = await _db.Students
                .Where(s => s.Id == request.Id)
                .AsNoTracking()
                .ProjectTo<StudentDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);


            return student;
        }
    }
}
