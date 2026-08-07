using Akkhor.Application.Interfaces.Repositories;
using Akkhor.Domain.Entities;
using Akkhor.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Akkhor.Infrastructure.Repositories;

public class ClassRepository : IClassRepository
{
    private readonly ApplicationDbContext _context;


    public ClassRepository(ApplicationDbContext context)
    {
        _context = context;
    }



    public async Task<IEnumerable<Class>> GetAllAsync()
    {
        return await _context.Classes
            .Include(x => x.AcademicYear)
            .Include(x => x.Sections)
            .OrderBy(x => x.DisplayOrder)
            .ThenBy(x => x.Name)
            .ToListAsync();
    }



    public async Task<Class?> GetByIdAsync(Guid id)
    {
        return await _context.Classes
            .Include(x => x.AcademicYear)
            .Include(x => x.Sections)
            .Include(x => x.Courses)
            .Include(x => x.StudentEnrollments)
            .FirstOrDefaultAsync(x => x.Id == id);
    }



    public async Task<Class?> GetByCodeAsync(string code)
    {
        return await _context.Classes
            .FirstOrDefaultAsync(x =>
                x.Code.ToLower() == code.ToLower());
    }



    public async Task AddAsync(Class entity)
    {
        await _context.Classes.AddAsync(entity);

        await _context.SaveChangesAsync();
    }



    public async Task UpdateAsync(Class entity)
    {
        _context.Classes.Update(entity);

        await _context.SaveChangesAsync();
    }



    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.Classes
            .FirstOrDefaultAsync(x => x.Id == id);


        if (entity != null)
        {
            _context.Classes.Remove(entity);

            await _context.SaveChangesAsync();
        }
    }



    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Classes
            .AnyAsync(x => x.Id == id);
    }
}