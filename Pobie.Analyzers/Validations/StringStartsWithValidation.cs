using System;

namespace Analyzers.Validations;

public class StringStartsWithValidation(String startsWith) : IValidation
{
    public bool Validate(object value)
    {
        return value is string valueString && valueString.StartsWith(startsWith);
    }
}
