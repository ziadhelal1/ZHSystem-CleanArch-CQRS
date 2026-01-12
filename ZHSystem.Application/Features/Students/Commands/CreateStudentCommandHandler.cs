using AutoMapper;
using MediatR;
using ZHSystem.Application.Common;
using ZHSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZHSystem.Application.DTOs;

namespace ZHSystem.Application.Features.Students.Commands
{
    public record CreateStudentCommand(CreateStudentDto dto):IRequest<int>;
    public class CreateStudentCommandHandler:IRequest<CreateStudentCommand>
    {

       private readonly IApplicationDbContext _db;
        private readonly IMapper _mapper;
        public CreateStudentCommandHandler(IApplicationDbContext db,IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<int> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            var student =_mapper.Map<Student>(request.dto);

            
            _db.Students.Add(student);
            await _db.SaveChangesAsync(cancellationToken);
            return student.Id;
        }

    }
}
