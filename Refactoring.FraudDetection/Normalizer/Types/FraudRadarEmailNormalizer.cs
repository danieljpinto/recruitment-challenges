// <copyright file="FraudRadar.cs" company="Payvision">
// Copyright (c) Payvision. All rights reserved.
// </copyright>

namespace Refactoring.FraudDetection.Normalizer.Types
{
    using Refactoring.FraudDetection.Model;
    using System;

    public class FraudRadarEmailNormalizer : FraudRadarNormalizer<Order>
    {
        public override void Normalize(Order order)
        {
            if (order?.Email != null)
            {
                var aux = order.Email.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);

                var atIndex = aux[0].IndexOf("+", StringComparison.Ordinal);

                aux[0] = atIndex < 0 ? aux[0].Replace(".", "") : aux[0].Replace(".", "").Remove(atIndex);

                order.Email = string.Join("@", new string[] { aux[0], aux[1] });
            }

            if (next != null)
            {
                next.Normalize(order);
            }
        }
    }
}