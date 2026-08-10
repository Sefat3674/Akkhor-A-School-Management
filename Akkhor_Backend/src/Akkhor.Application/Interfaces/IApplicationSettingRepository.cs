
using Akkhor.Domain.Entities;

namespace Akkhor.Application.Interfaces.Repositories;

public interface IApplicationSettingRepository
{
    Task<IEnumerable<ApplicationSetting>> GetAllAsync();

    Task<ApplicationSetting?> GetByIdAsync(Guid id);

    Task<ApplicationSetting?> GetByKeyAsync(string key);

    Task<IEnumerable<ApplicationSetting>> GetByCategoryAsync(string category);

    Task<ApplicationSetting> AddAsync(ApplicationSetting setting);

    Task<ApplicationSetting> UpdateAsync(ApplicationSetting setting);

    Task<bool> DeleteAsync(Guid id);

    Task<bool> ExistsByKeyAsync(string key);
}

