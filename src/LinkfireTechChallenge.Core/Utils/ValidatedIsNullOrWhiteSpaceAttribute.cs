﻿using System;

namespace LinkfireTechChallenge.Core.Utils
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class ValidatedIsNullOrWhiteSpaceAttribute : Attribute { }
}
