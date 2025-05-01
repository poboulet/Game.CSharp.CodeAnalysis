using System.Linq;

namespace Pobie.Roslyn.Validations;

public class StringDoesNotEndWithPunctuation : IValidation
{
    public bool Validate(object value)
    {
        if (value is not string { Length: > 0 } valueString)
        {
            return true;
        }

        return !char.IsPunctuation(valueString.Last());
    }
}
