// <copyright file="FraudRadar.cs" company="Payvision">
// Copyright (c) Payvision. All rights reserved.
// </copyright>

namespace Refactoring.FraudDetection.Normalizer
{
    using System.Collections.Generic;

    public interface IFraudRadarNormalizer<T>
    {
        IEnumerable<T> Normalize(IEnumerable<T> items);
    }
}