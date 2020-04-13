// <copyright file="FraudRadar.cs" company="Payvision">
// Copyright (c) Payvision. All rights reserved.
// </copyright>

using Refactoring.FraudDetection.Model;

namespace Refactoring.FraudDetection.Normalizer.Types
{
    public class FraudRadarStateNormalizer : FraudRadarNormalizer<Order>
    {
        public override void Normalize(Order order)
        {
            order.State = order.State.Replace("il", "illinois").Replace("ca", "california").Replace("ny", "new york");

            if (next != null)
            {
                next.Normalize(order);
            }
        }
    }
}