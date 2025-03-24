namespace Pobie.Analyzers.Validations;

public interface IValidation
{
    public bool Validate(object value);
}
