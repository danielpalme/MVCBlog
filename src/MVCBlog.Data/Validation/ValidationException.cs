using System;
using System.Collections.Generic;
using System.Data;

namespace MVCBlog.Data.Validation
{
    [Serializable]
    public class ValidationException : DataException
    {
        public ValidationException()
        {
        }

        public ValidationException(IEnumerable<EntityValidationResult> validationResults)
        {
            this.ValidationResults = validationResults;
        }

        public IEnumerable<EntityValidationResult> ValidationResults { get; }
    }
}
