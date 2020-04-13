// <copyright file="FraudRadar.cs" company="Payvision">
// Copyright (c) Payvision. All rights reserved.
// </copyright>

namespace Refactoring.FraudDetection.Rules
{
    public interface IFraudCheckerService<T>
    {
        bool ValidateRule(T current, T next);
    }
}