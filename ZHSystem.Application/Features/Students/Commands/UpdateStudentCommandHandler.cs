using MediatR;


namespace ZHSystem.Application.Features.Students.Commands;


public record UpdateStudentCommand(int Id, string FirstName, string LastName, DateTime DateOfBirth, string? Email) : IRequest<bool>;


public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, bool>
{
    private readonly Common.IApplicationDbContext _db;
    public UpdateStudentCommandHandler(Common.IApplicationDbContext db) => _db = db;


    public async Task<bool> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
    {
        var student = await _db.Students.FindAsync(new object[] { request.Id }, cancellationToken);
        if (student is null || student.IsDeleted) return false;


        student.FirstName = request.FirstName;
        student.LastName = request.LastName;
        student.DateOfBirth = request.DateOfBirth;
        student.Email = request.Email;


        
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}