﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bifrost.Rules;
using Bifrost.Validation.Rules;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Bifrost.Specs.Validation.Rules.for_Required
{
    public class when_evaluating_empty_string
    {
        static Required rule;
        static Mock<IRuleContext> rule_context_mock;
        static Exception exception;

        Establish context = () =>
        {
            rule = new Required(null);
            rule_context_mock = new Mock<IRuleContext>();
        };

        Because of = () => rule.Evaluate(rule_context_mock.Object, string.Empty);

        It should_fail_with_string_is_empty_as_reason = () => rule_context_mock.Verify(r => r.Fail(rule, string.Empty, Required.StringIsEmpty));
    }
}
