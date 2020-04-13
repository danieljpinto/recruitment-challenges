// <copyright file="FraudRadar.cs" company="Payvision">
// Copyright (c) Payvision. All rights reserved.
// </copyright>

using Refactoring.FraudDetection.Model;

namespace Refactoring.FraudDetection.Normalizer.Types
{
    public class FraudRadarStreetNormalizer : FraudRadarNormalizer<Order>
    {
        public override void Normalize(Order order)
        {
            if (order?.Street != null)
            {
                order.Street = order.Street.Replace("st.", "street").Replace("rd.", "road");
            }

            if (next != null)
            {
                next.Normalize(order);
            }
        }
    }
}