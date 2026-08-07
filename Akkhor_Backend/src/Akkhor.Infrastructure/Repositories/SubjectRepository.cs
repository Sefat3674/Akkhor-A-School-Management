using Akkhor.Application.Interfaces.Repositories;
using Akkhor.Domain.Entities;
using Akkhor.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Akkhor.Infrastructure.Repositories;

public class SubjectRepository : ISubjectRepository
{
    private readonly ApplicationDbContext _context;

    public SubjectRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Subject>> GetAllAsync()
    {
        return await _context.Subjects
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public async Task<Subject?> GetByIdAsync(Guid id)
    {
        return await _context.Subjects
            .Include(x => x.CourseSubjects)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Subject?> GetByCodeAsync(string code)
    {
        return await _context.Subjects
            .FirstOrDefaultAsync(x =>
                x.Code.ToLower() == code.ToLower());
    }

    public async Task AddAsync(Subject entity)
    {
        await _context.Subjects.AddAsync(entity);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Subject entity)
    {
        _context.Subjects.Update(entity);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.Subjects
            .FirstOrDefaultAsync(x => x.Id == id);

        if (entity != null)
        {
            _context.Subjects.Remove(entity);

            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Subjects
            .AnyAsync(x => x.Id == id);
    }
}