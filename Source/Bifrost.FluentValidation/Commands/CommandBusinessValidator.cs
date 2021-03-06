﻿#region License
//
// Copyright (c) 2008-2015, Dolittle (http://www.dolittle.com)
//
// Licensed under the MIT License (http://opensource.org/licenses/MIT)
//
// You may not use this file except in compliance with the License.
// You may obtain a copy of the license at 
//
//   http://github.com/dolittle/Bifrost/blob/master/MIT-LICENSE.txt
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Bifrost.Commands;
using Bifrost.Validation;
using FluentValidation;
using FluentValidation.Internal;

namespace Bifrost.FluentValidation.Commands
{
    /// <summary>
    /// Base class to inherit from for basic business-rule validation of a command.
    /// </summary>
    /// <remarks>
    /// Commands inherting from this base class will be automatically registered.
    /// </remarks>
    /// <typeparam name="T">Concrete type of the Command to validate</typeparam>
    public abstract class CommandBusinessValidator<T> : BusinessValidator<T>, ICanValidate<T>, ICommandBusinessValidator where T : class, ICommand
    {
#pragma warning disable 1591 // Xml Comments
        public virtual IEnumerable<ValidationResult> ValidateFor(T command)
        {
            var result = Validate(command as T);
            return from error in result.Errors
                   select new ValidationResult(error.ErrorMessage, new[] { error.PropertyName });
        }

        IEnumerable<ValidationResult> ICanValidate.ValidateFor(object target)
        {
            return ValidateFor((T)target);
        }
#pragma warning restore 1591 // Xml Comments

        /// <summary>
        /// Add a predicate rule based on a Func that will be called when validation occurs
        /// </summary>
        /// <param name="validateFor"><see cref="Func{T, TR}"/> that will be called for validation</param>
        /// <returns><see cref="IRuleBuilderOptions{T, TR}"/> that can be used to fluently configure options for the rule</returns>
        public IRuleBuilderOptions<T, object> AddRule(Func<T, bool> validateFor)
        {
            var rule = CommandPredicateRule<T>.Create(validateFor);
            AddRule(rule);

            var ruleBuilder = new RuleBuilder<T, object>(rule);
            return ruleBuilder;
        }
    }
}