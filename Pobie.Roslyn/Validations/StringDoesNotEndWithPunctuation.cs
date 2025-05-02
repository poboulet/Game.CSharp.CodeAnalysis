using System.Linq;

namespace Pobie.Roslyn.Validations;

public class StringDoesNotEndWithPunctuation : IValidation
{
    private readonly char[] _forbiddenPunctuation = ['.', '!', '?', ','];

    public bool Validate(object value)
    {
        return value is not string { Length: > 0 } valueString
            || _forbiddenPunctuation.All(punctuation => punctuation != valueString.Last());
    }
}
