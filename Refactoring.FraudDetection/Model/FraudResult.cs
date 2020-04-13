// <copyright file="FraudRadar.cs" company="Payvision">
// Copyright (c) Payvision. All rights reserved.
// </copyright>

namespace Refactoring.FraudDetection.Model
{
    public class FraudResult
    {
        public int OrderId { get; set; }

        public bool IsFraudulent { get; set; }
    }
}