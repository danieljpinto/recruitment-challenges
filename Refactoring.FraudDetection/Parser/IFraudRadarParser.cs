// <copyright file="FraudRadar.cs" company="Payvision">
// Copyright (c) Payvision. All rights reserved.
// </copyright>

namespace Refactoring.FraudDetection.Parser
{
    using System.Collections.Generic;

    public interface IFraudRadarParser<T>
    {
        IEnumerable<T> Parse();
    }
}