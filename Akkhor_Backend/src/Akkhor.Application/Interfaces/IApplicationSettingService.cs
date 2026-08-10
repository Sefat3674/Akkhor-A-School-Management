
using Akkhor.Application.DTOs.ApplicationSettings;

namespace Akkhor.Application.Interfaces.Services;

public interface IApplicationSettingService
{
    // =====================================================
    // GET ALL
    // =====================================================

    Task<IEnumerable<ApplicationSettingDto>> GetAllAsync();


    // =====================================================
    // GET BY ID
    // =====================================================

    Task<ApplicationSettingDto?> GetByIdAsync(Guid id);


    // =====================================================
    // GET BY KEY
    // =====================================================

    Task<ApplicationSettingDto?> GetByKeyAsync(string key);


    // =====================================================
    // GET BY CATEGORY
    // =====================================================

    Task<IEnumerable<ApplicationSettingDto>> GetByCategoryAsync(
        string category);


    // =====================================================
    // CREATE
    // =====================================================

    Task<ApplicationSettingDto> CreateAsync(
        CreateApplicationSettingDto dto);


    // =====================================================
    // UPDATE
    // =====================================================

    Task<ApplicationSettingDto?> UpdateAsync(
        Guid id,
        UpdateApplicationSettingDto dto);


    // =====================================================
    // DELETE
    // =====================================================

    Task<bool> DeleteAsync(Guid id);


    // =====================================================
    // GET SETTING VALUE
    // =====================================================

    Task<string?> GetValueAsync(string key);


    // =====================================================
    // GET TYPED SETTING VALUE
    // =====================================================

    Task<T?> GetValueAsync<T>(string key);
}

