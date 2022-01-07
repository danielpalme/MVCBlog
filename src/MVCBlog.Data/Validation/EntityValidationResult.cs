using System.ComponentModel.DataAnnotations;

namespace MVCBlog.Data.Validation;

public class EntityValidationResult
{
    public EntityValidationResult(object entity, List<ValidationResult> validationResults)
    {
        this.Entity = entity;
        this.ValidationErrors = validationResults;
    }

    public object Entity { get; set; }

    public ICollection<ValidationResult> ValidationErrors { get; }
}