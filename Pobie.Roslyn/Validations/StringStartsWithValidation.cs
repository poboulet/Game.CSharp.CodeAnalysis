using System.Linq;

namespace Pobie.Roslyn.Validations;

public class StringStartsWithValidation(string startsWith) : IValidation
{
    public bool Validate(object value)
    {
        if (value is string valueString)
        {
            return valueString.First() == '$'
                ? valueString.Remove(0, 1).StartsWith(startsWith)
                : valueString.StartsWith(startsWith);
        }

        return false;
    }
}
