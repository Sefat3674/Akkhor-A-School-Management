using Akkhor.Application.Interfaces.Repositories;
using Akkhor.Domain.Entities;
using Akkhor.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Akkhor.Infrastructure.Repositories;

public class AcademicYearRepository
    : IAcademicYearRepository
{
    private readonly ApplicationDbContext _context;


    public AcademicYearRepository(
        ApplicationDbContext context)
    {
        _context = context;
    }



    public async Task<IEnumerable<AcademicYear>> GetAllAsync()
    {
        return await _context.AcademicYears
            .OrderByDescending(x => x.StartDate)
            .ToListAsync();
    }



    public async Task<AcademicYear?> GetByIdAsync(Guid id)
    {
        return await _context.AcademicYears
            .FirstOrDefaultAsync(x => x.Id == id);
    }



    public async Task<bool> ExistsAsync(string name)
    {
        return await _context.AcademicYears
            .AnyAsync(x => x.Name == name);
    }



    public async Task AddAsync(
        AcademicYear academicYear)
    {
        academicYear.CreatedAt = DateTime.UtcNow;

        await _context.AcademicYears
            .AddAsync(academicYear);
    }



    public Task UpdateAsync(
        AcademicYear academicYear)
    {
        academicYear.UpdatedAt = DateTime.UtcNow;

        _context.AcademicYears
            .Update(academicYear);

        return Task.CompletedTask;
    }



    public Task DeleteAsync(
        AcademicYear academicYear)
    {
        _context.AcademicYears
            .Remove(academicYear);

        return Task.CompletedTask;
    }



    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}