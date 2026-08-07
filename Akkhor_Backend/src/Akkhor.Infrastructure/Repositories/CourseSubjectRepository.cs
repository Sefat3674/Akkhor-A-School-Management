using Akkhor.Application.Interfaces.Repositories;
using Akkhor.Domain.Entities;
using Akkhor.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Akkhor.Infrastructure.Repositories;

public class CourseSubjectRepository : ICourseSubjectRepository
{
    private readonly ApplicationDbContext _context;

    public CourseSubjectRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CourseSubject>> GetAllAsync()
    {
        return await _context.CourseSubjects
            .Include(x => x.Course)
            .Include(x => x.Subject)
            .OrderBy(x => x.DisplayOrder)
            .ToListAsync();
    }

    public async Task<CourseSubject?> GetByIdAsync(Guid id)
    {
        return await _context.CourseSubjects
            .Include(x => x.Course)
            .Include(x => x.Subject)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(CourseSubject entity)
    {
        entity.Id = Guid.NewGuid();
        entity.CreatedAt = DateTime.UtcNow;

        await _context.CourseSubjects.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CourseSubject entity)
    {
        _context.CourseSubjects.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.CourseSubjects.FindAsync(id);

        if (entity == null)
            return;

        _context.CourseSubjects.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid courseId, Guid subjectId)
    {
        return await _context.CourseSubjects.AnyAsync(x =>
            x.CourseId == courseId &&
            x.SubjectId == subjectId);
    }
}