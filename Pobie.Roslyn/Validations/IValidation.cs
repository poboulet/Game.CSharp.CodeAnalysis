namespace Pobie.Roslyn.Validations;

public interface IValidation
{
    public bool Validate(object value);
}
