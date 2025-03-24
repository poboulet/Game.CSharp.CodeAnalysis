using System;

namespace Analyzers.Validations;

public interface IValidation
{
    public bool Validate(Object value);
}
