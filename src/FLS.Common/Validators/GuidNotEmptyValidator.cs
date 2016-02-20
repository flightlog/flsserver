using System;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace FLS.Common.Validators
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public sealed class GuidNotEmptyValidatorAttribute : ValueValidatorAttribute
    {
        protected override Validator DoCreateValidator(Type targetType)
        {
            return new GuidNotEmptyValidator();
        }
    }

    internal sealed class GuidNotEmptyValidator : ValueValidator<Guid>
    {
        public GuidNotEmptyValidator()
            : base(null, null, false)
        { }

        public override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {

            if (objectToValidate == null) { throw new ArgumentNullException("objectToValidate"); }

            this.DoValidate(new Guid(objectToValidate.ToString()), currentTarget, key, validationResults);
        }

        protected override void DoValidate(Guid objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            if (objectToValidate == Guid.Empty)
            {
                this.LogValidationResult(validationResults, this.GetMessage(objectToValidate, key), currentTarget, key);
            }
        }

        protected override string DefaultNonNegatedMessageTemplate
        {
            get { return "Return a message about how this Guid is empty and shouldn't be."; }
        }

        protected override string DefaultNegatedMessageTemplate
        {
            get { return "Return a message about how Guid should be empty."; }
        }

        protected override string GetMessage(object objectToValidate, string key)
        {
            return base.GetMessage(objectToValidate, key);
        }
    }
}
