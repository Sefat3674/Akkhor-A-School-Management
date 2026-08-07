using Akkhor.Application.Interfaces.Repositories;
using Akkhor.Domain.Entities;
using Akkhor.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Akkhor.Infrastructure.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly ApplicationDbContext _context;


    public CourseRepository(ApplicationDbContext context)
    {
        _context = context;
    }



    public async Task<IEnumerable<Course>> GetAllAsync()
    {
        return await _context.Courses

            .Include(x => x.Class)

            .Include(x => x.CourseSubjects)

            .Include(x => x.StudentEnrollments)

            .OrderBy(x => x.CourseName)

            .ToListAsync();
    }





    public async Task<Course?> GetByIdAsync(Guid id)
    {
        return await _context.Courses

            .Include(x => x.Class)

            .Include(x => x.CourseSubjects)

            .Include(x => x.StudentEnrollments)

            .FirstOrDefaultAsync(x => x.Id == id);
    }





    public async Task<Course?> GetByCodeAsync(string code)
    {
        return await _context.Courses

            .FirstOrDefaultAsync(x =>
                x.CourseCode.ToLower() == code.ToLower());
    }





    public async Task AddAsync(Course entity)
    {
        await _context.Courses.AddAsync(entity);

        await _context.SaveChangesAsync();
    }





    public async Task UpdateAsync(Course entity)
    {
        _context.Courses.Update(entity);

        await _context.SaveChangesAsync();
    }





    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.Courses

            .FirstOrDefaultAsync(x => x.Id == id);



        if (entity != null)
        {
            _context.Courses.Remove(entity);

            await _context.SaveChangesAsync();
        }
    }





    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Courses

            .AnyAsync(x => x.Id == id);
    }
}