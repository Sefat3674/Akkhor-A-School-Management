using Akkhor.Application.Interfaces.Repositories;
using Akkhor.Domain.Entities;
using Akkhor.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Akkhor.Infrastructure.Repositories;

public class SectionRepository : ISectionRepository
{
    private readonly ApplicationDbContext _context;

    public SectionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ClassSection>> GetAllAsync()
    {
        return await _context.ClassSections
            .Include(x => x.Class)
            .Include(x => x.StudentEnrollments)
            .OrderBy(x => x.SectionName)
            .ToListAsync();
    }

    public async Task<ClassSection?> GetByIdAsync(Guid id)
    {
        return await _context.ClassSections
            .Include(x => x.Class)
            .Include(x => x.StudentEnrollments)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ClassSection?> GetByNameAsync(
        Guid classId,
        string sectionName)
    {
        return await _context.ClassSections
            .FirstOrDefaultAsync(x =>
                x.ClassId == classId &&
                x.SectionName.ToLower() == sectionName.ToLower());
    }

    public async Task AddAsync(ClassSection entity)
    {
        await _context.ClassSections.AddAsync(entity);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ClassSection entity)
    {
        _context.ClassSections.Update(entity);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.ClassSections
            .FirstOrDefaultAsync(x => x.Id == id);

        if (entity != null)
        {
            _context.ClassSections.Remove(entity);

            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.ClassSections
            .AnyAsync(x => x.Id == id);
    }
}