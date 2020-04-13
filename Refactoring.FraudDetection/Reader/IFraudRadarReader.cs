// <copyright file="FraudRadar.cs" company="Payvision">
// Copyright (c) Payvision. All rights reserved.
// </copyright>

namespace Refactoring.FraudDetection.Reader
{
    using System.Collections.Generic;

    public interface IFraudRadarReader
    {
        IEnumerable<string> ReadAll();
    }
}