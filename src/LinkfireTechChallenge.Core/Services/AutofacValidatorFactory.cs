using Autofac;
using FluentValidation;
using System;

namespace LinkfireTechChallenge.Core.Services
{
    public class AutofacValidatorFactory : ValidatorFactoryBase
    {
        private readonly ILifetimeScope _scope;

        public AutofacValidatorFactory(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public override IValidator CreateInstance(Type validatorType)
        {
            return _scope.Resolve(validatorType) as IValidator;
        }
    }
}
