// <copyright file="FraudRadar.cs" company="Payvision">
// Copyright (c) Payvision. All rights reserved.
// </copyright>

namespace Refactoring.FraudDetection.Normalizer
{
    using Refactoring.FraudDetection.Model;
    using Refactoring.FraudDetection.Normalizer.Types;
    using System.Collections.Generic;

    public class FraudRadarOrderNormalizer : IFraudRadarNormalizer<Order>
    {
        public IEnumerable<Order> Normalize(IEnumerable<Order> orders)
        {
            // Use of Chain of Resp., could be done in a several ways
            var n1 = new FraudRadarEmailNormalizer();
            var n2 = new FraudRadarStreetNormalizer();
            var n3 = new FraudRadarStateNormalizer();

            n1.SetNextNormalizer(n2);
            n2.SetNextNormalizer(n3);

            foreach (var order in orders)
            {
                n1.Normalize(order);
            }

            return orders;
        }
    }
}