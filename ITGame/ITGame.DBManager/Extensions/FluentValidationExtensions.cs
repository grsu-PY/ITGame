using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Internal;

namespace ITGame.DBManager.Extensions
{
    public static class FluentValidationExtensions
    {
        public static IEnumerable<PropertyRule> AllPropertyRules(this IValidator validator)
        {
            var descriptor = validator.CreateDescriptor();
            return
                (from member in descriptor.GetMembersWithValidators()
                    from validationRule in descriptor.GetRulesForMember(member.Key)
                    select validationRule).Cast<PropertyRule>();
        }
    }
}