using Akkhor.Application.DTOs.Classes;
using Akkhor.Application.Interfaces.Repositories;
using Akkhor.Application.Interfaces.Services;
using Akkhor.Domain.Entities;

namespace Akkhor.Application.Services;

public class ClassService : IClassService
{
    private readonly IClassRepository _repository;


    public ClassService(IClassRepository repository)
    {
        _repository = repository;
    }



    public async Task<IEnumerable<ClassDto>> GetAllAsync()
    {
        var classes = await _repository.GetAllAsync();


        return classes.Select(x => new ClassDto
        {
            Id = x.Id,

            AcademicYearId = x.AcademicYearId,

            AcademicYearName =
                x.AcademicYear?.Name ?? "",

            Name = x.Name,

            Code = x.Code,

            Description = x.Description,

            DisplayOrder = x.DisplayOrder,

            IsActive = x.IsActive,

            SectionCount = x.Sections.Count,

            CreatedAt = x.CreatedAt
        });
    }





    public async Task<ClassDto?> GetByIdAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);


        if (entity == null)
            return null;



        return new ClassDto
        {
            Id = entity.Id,

            AcademicYearId = entity.AcademicYearId,

            AcademicYearName =
                entity.AcademicYear?.Name ?? "",

            Name = entity.Name,

            Code = entity.Code,

            Description = entity.Description,

            DisplayOrder = entity.DisplayOrder,

            IsActive = entity.IsActive,

            SectionCount = entity.Sections.Count,

            CreatedAt = entity.CreatedAt
        };
    }





    public async Task<ClassDto> CreateAsync(CreateClassDto dto)
    {

        var exists = await _repository
            .GetByCodeAsync(dto.Code);


        if (exists != null)
            throw new Exception("Class code already exists");



        var entity = new Class
        {
            Id = Guid.NewGuid(),

            AcademicYearId = dto.AcademicYearId,

            Name = dto.Name,

            Code = dto.Code,

            Description = dto.Description,

            DisplayOrder = dto.DisplayOrder,

            IsActive = true,

            CreatedAt = DateTime.UtcNow
        };



        await _repository.AddAsync(entity);



        return await GetByIdAsync(entity.Id)
            ?? throw new Exception("Unable to create class");
    }





    public async Task<bool> UpdateAsync(
        Guid id,
        UpdateClassDto dto)
    {

        var entity = await _repository
            .GetByIdAsync(id);



        if (entity == null)
            return false;



        entity.Name = dto.Name;
        entity.Code = dto.Code;
        entity.Description = dto.Description;
        entity.DisplayOrder = dto.DisplayOrder;
        entity.IsActive = dto.IsActive;

        entity.CreatedAt =
            DateTime.SpecifyKind(entity.CreatedAt, DateTimeKind.Utc);

        entity.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(entity);


        return true;
    }





    public async Task<bool> DeleteAsync(Guid id)
    {

        var exists = await _repository
            .ExistsAsync(id);



        if (!exists)
            return false;



        await _repository.DeleteAsync(id);


        return true;
    }
}