using MediatR;


namespace ZHSystem.Application.Features.Students.Commands;


public record DeleteStudentCommand(int Id) : IRequest<bool>;


public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand, bool>
{
    private readonly Common.IApplicationDbContext _db;
    public DeleteStudentCommandHandler(Common.IApplicationDbContext db) => _db = db;


    public async Task<bool> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
    {
        var student = await _db.Students.FindAsync(new object[] { request.Id }, cancellationToken);
        if (student is null || student.IsDeleted) return false;


        student.IsDeleted = true;
        student.DeletedAt = DateTime.UtcNow;
        _db.Students.Update(student);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}