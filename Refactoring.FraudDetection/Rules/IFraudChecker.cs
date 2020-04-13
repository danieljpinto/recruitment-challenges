// <copyright file="FraudRadar.cs" company="Payvision">
// Copyright (c) Payvision. All rights reserved.
// </copyright>

namespace Refactoring.FraudDetection.Rules
{
    public interface IFraudChecker<T>
    {
        bool IsFraudulent(T current, T next);
    }
}