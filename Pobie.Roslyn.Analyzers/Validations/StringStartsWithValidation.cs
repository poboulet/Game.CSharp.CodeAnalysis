namespace Pobie.Analyzers.Validations;

public class StringStartsWithValidation(string startsWith) : IValidation
{
    public bool Validate(object value)
    {
        return value is string valueString && valueString.StartsWith(startsWith);
    }
}
