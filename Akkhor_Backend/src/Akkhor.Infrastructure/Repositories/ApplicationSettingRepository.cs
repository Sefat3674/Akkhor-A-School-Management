
using Akkhor.Application.Interfaces.Repositories;
using Akkhor.Domain.Entities;
using Akkhor.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Akkhor.Infrastructure.Repositories;

public class ApplicationSettingRepository : IApplicationSettingRepository
{
    private readonly ApplicationDbContext _context;

    public ApplicationSettingRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // =====================================================
    // GET ALL
    // =====================================================

    public async Task<IEnumerable<ApplicationSetting>> GetAllAsync()
    {
        return await _context.ApplicationSettings
            .AsNoTracking()
            .OrderBy(x => x.Category)
            .ThenBy(x => x.Key)
            .ToListAsync();
    }

    // =====================================================
    // GET BY ID
    // =====================================================

    public async Task<ApplicationSetting?> GetByIdAsync(Guid id)
    {
        return await _context.ApplicationSettings
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    // =====================================================
    // GET BY KEY
    // =====================================================

    public async Task<ApplicationSetting?> GetByKeyAsync(string key)
    {
        return await _context.ApplicationSettings
            .FirstOrDefaultAsync(x => x.Key == key);
    }

    // =====================================================
    // GET BY CATEGORY
    // =====================================================

    public async Task<IEnumerable<ApplicationSetting>> GetByCategoryAsync(
        string category)
    {
        return await _context.ApplicationSettings
            .AsNoTracking()
            .Where(x => x.Category == category)
            .OrderBy(x => x.Key)
            .ToListAsync();
    }

    // =====================================================
    // ADD
    // =====================================================

    public async Task<ApplicationSetting> AddAsync(
        ApplicationSetting setting)
    {
        if (setting.Id == Guid.Empty)
        {
            setting.Id = Guid.NewGuid();
        }

        setting.CreatedAt = DateTime.UtcNow;
        setting.UpdatedAt = null;

        _context.ApplicationSettings.Add(setting);

        await _context.SaveChangesAsync();

        return setting;
    }

    // =====================================================
    // UPDATE
    // =====================================================

    public async Task<ApplicationSetting> UpdateAsync(
        ApplicationSetting setting)
    {
        // Make sure UpdatedAt is always UTC
        setting.UpdatedAt = DateTime.UtcNow;

        // The entity was already loaded by GetByIdAsync(),
        // so EF is already tracking it.
        //
        // Do NOT call:
        // _context.ApplicationSettings.Update(setting);
        //
        // because that marks every property as modified.

        _context.Entry(setting).Property(x => x.Key).IsModified = true;
        _context.Entry(setting).Property(x => x.Value).IsModified = true;
        _context.Entry(setting).Property(x => x.Category).IsModified = true;
        _context.Entry(setting).Property(x => x.DataType).IsModified = true;
        _context.Entry(setting).Property(x => x.Description).IsModified = true;
        _context.Entry(setting).Property(x => x.IsActive).IsModified = true;
        _context.Entry(setting).Property(x => x.UpdatedAt).IsModified = true;
        _context.Entry(setting).Property(x => x.UpdatedBy).IsModified = true;

        // CreatedAt and Id are intentionally NOT modified.

        await _context.SaveChangesAsync();

        return setting;
    }

    // =====================================================
    // DELETE
    // =====================================================

    public async Task<bool> DeleteAsync(Guid id)
    {
        var setting = await _context.ApplicationSettings
            .FirstOrDefaultAsync(x => x.Id == id);

        if (setting == null)
        {
            return false;
        }

        _context.ApplicationSettings.Remove(setting);

        await _context.SaveChangesAsync();

        return true;
    }

    // =====================================================
    // EXISTS BY KEY
    // =====================================================

    public async Task<bool> ExistsByKeyAsync(string key)
    {
        return await _context.ApplicationSettings
            .AnyAsync(x => x.Key == key);
    }
}

