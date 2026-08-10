
using Akkhor.Application.DTOs.ApplicationSettings;
using Akkhor.Application.Interfaces.Repositories;
using Akkhor.Application.Interfaces.Services;
using Akkhor.Domain.Entities;

namespace Akkhor.Application.Services;

public class ApplicationSettingService : IApplicationSettingService
{
    private readonly IApplicationSettingRepository _repository;

    public ApplicationSettingService(
        IApplicationSettingRepository repository)
    {
        _repository = repository;
    }

    // =====================================================
    // GET ALL
    // =====================================================

    public async Task<IEnumerable<ApplicationSettingDto>> GetAllAsync()
    {
        var settings = await _repository.GetAllAsync();

        return settings.Select(MapToDto);
    }

    // =====================================================
    // GET BY ID
    // =====================================================

    public async Task<ApplicationSettingDto?> GetByIdAsync(Guid id)
    {
        var setting = await _repository.GetByIdAsync(id);

        return setting == null
            ? null
            : MapToDto(setting);
    }

    // =====================================================
    // GET BY KEY
    // =====================================================

    public async Task<ApplicationSettingDto?> GetByKeyAsync(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return null;
        }

        var setting = await _repository.GetByKeyAsync(key.Trim());

        return setting == null
            ? null
            : MapToDto(setting);
    }

    // =====================================================
    // GET BY CATEGORY
    // =====================================================

    public async Task<IEnumerable<ApplicationSettingDto>> GetByCategoryAsync(
        string category)
    {
        if (string.IsNullOrWhiteSpace(category))
        {
            return Enumerable.Empty<ApplicationSettingDto>();
        }

        var settings = await _repository.GetByCategoryAsync(
            category.Trim());

        return settings.Select(MapToDto);
    }

    // =====================================================
    // CREATE
    // =====================================================

    public async Task<ApplicationSettingDto> CreateAsync(
        CreateApplicationSettingDto dto)
    {
        if (dto == null)
        {
            throw new ArgumentNullException(nameof(dto));
        }

        if (string.IsNullOrWhiteSpace(dto.Key))
        {
            throw new ArgumentException(
                "Setting key is required.",
                nameof(dto.Key));
        }

        var key = dto.Key.Trim();

        var exists = await _repository.ExistsByKeyAsync(key);

        if (exists)
        {
            throw new InvalidOperationException(
                $"A setting with key '{key}' already exists.");
        }

        var setting = new ApplicationSetting
        {
            Id = Guid.NewGuid(),

            Key = key,

            Value = dto.Value,

            Category = string.IsNullOrWhiteSpace(dto.Category)
                ? "General"
                : dto.Category.Trim(),

            DataType = string.IsNullOrWhiteSpace(dto.DataType)
                ? "string"
                : dto.DataType.Trim(),

            Description = string.IsNullOrWhiteSpace(dto.Description)
                ? null
                : dto.Description.Trim(),

            IsActive = dto.IsActive,

            CreatedAt = DateTime.UtcNow
        };

        var created = await _repository.AddAsync(setting);

        return MapToDto(created);
    }

    // =====================================================
    // UPDATE
    // =====================================================

    public async Task<ApplicationSettingDto?> UpdateAsync(
        Guid id,
        UpdateApplicationSettingDto dto)
    {
        if (dto == null)
        {
            throw new ArgumentNullException(nameof(dto));
        }

        var setting = await _repository.GetByIdAsync(id);

        if (setting == null)
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(dto.Key))
        {
            throw new ArgumentException(
                "Setting key is required.",
                nameof(dto.Key));
        }

        var newKey = dto.Key.Trim();

        // Check whether another setting already uses this key
        if (!string.Equals(
                setting.Key,
                newKey,
                StringComparison.OrdinalIgnoreCase))
        {
            var keyExists = await _repository.ExistsByKeyAsync(newKey);

            if (keyExists)
            {
                throw new InvalidOperationException(
                    $"A setting with key '{newKey}' already exists.");
            }
        }

        setting.Key = newKey;

        setting.Value = dto.Value;

        setting.Category = string.IsNullOrWhiteSpace(dto.Category)
            ? "General"
            : dto.Category.Trim();

        setting.DataType = string.IsNullOrWhiteSpace(dto.DataType)
            ? "string"
            : dto.DataType.Trim();

        setting.Description = string.IsNullOrWhiteSpace(dto.Description)
            ? null
            : dto.Description.Trim();

        setting.IsActive = dto.IsActive;

        setting.UpdatedAt = DateTime.UtcNow;

        var updated = await _repository.UpdateAsync(setting);

        return MapToDto(updated);
    }

    // =====================================================
    // DELETE
    // =====================================================

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }

    // =====================================================
    // GET STRING VALUE
    // =====================================================

    public async Task<string?> GetValueAsync(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return null;
        }

        var setting = await _repository.GetByKeyAsync(key.Trim());

        if (setting == null || !setting.IsActive)
        {
            return null;
        }

        return setting.Value;
    }

    // =====================================================
    // GET TYPED VALUE
    // =====================================================

    public async Task<T?> GetValueAsync<T>(string key)
    {
        var value = await GetValueAsync(key);

        if (string.IsNullOrWhiteSpace(value))
        {
            return default;
        }

        try
        {
            var targetType = Nullable.GetUnderlyingType(typeof(T))
                             ?? typeof(T);

            // String
            if (targetType == typeof(string))
            {
                return (T)(object)value;
            }

            // Boolean
            if (targetType == typeof(bool))
            {
                if (bool.TryParse(value, out var boolValue))
                {
                    return (T)(object)boolValue;
                }

                return default;
            }

            // Integer
            if (targetType == typeof(int))
            {
                if (int.TryParse(value, out var intValue))
                {
                    return (T)(object)intValue;
                }

                return default;
            }

            // Long
            if (targetType == typeof(long))
            {
                if (long.TryParse(value, out var longValue))
                {
                    return (T)(object)longValue;
                }

                return default;
            }

            // Decimal
            if (targetType == typeof(decimal))
            {
                if (decimal.TryParse(value, out var decimalValue))
                {
                    return (T)(object)decimalValue;
                }

                return default;
            }

            // Double
            if (targetType == typeof(double))
            {
                if (double.TryParse(value, out var doubleValue))
                {
                    return (T)(object)doubleValue;
                }

                return default;
            }

            // Guid
            if (targetType == typeof(Guid))
            {
                if (Guid.TryParse(value, out var guidValue))
                {
                    return (T)(object)guidValue;
                }

                return default;
            }

            // Enum
            if (targetType.IsEnum)
            {
                if (Enum.TryParse(
                        targetType,
                        value,
                        true,
                        out var enumValue))
                {
                    return (T)enumValue!;
                }

                return default;
            }

            // Other convertible types
            return (T)Convert.ChangeType(
                value,
                targetType);
        }
        catch
        {
            return default;
        }
    }

    // =====================================================
    // MAP ENTITY → DTO
    // =====================================================

    private static ApplicationSettingDto MapToDto(
        ApplicationSetting setting)
    {
        return new ApplicationSettingDto
        {
            Id = setting.Id,

            Key = setting.Key,

            Value = setting.Value,

            Category = setting.Category,

            DataType = setting.DataType,

            Description = setting.Description,

            IsActive = setting.IsActive,

            CreatedAt = setting.CreatedAt,

            UpdatedAt = setting.UpdatedAt,

            UpdatedBy = setting.UpdatedBy
        };
    }
}
