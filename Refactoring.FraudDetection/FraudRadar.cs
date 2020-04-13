// <copyright file="FraudRadar.cs" company="Payvision">
// Copyright (c) Payvision. All rights reserved.
// </copyright>

namespace Refactoring.FraudDetection
{
    using Refactoring.FraudDetection.Model;
    using Refactoring.FraudDetection.Rules;
    using System.Collections.Generic;
    using System.Linq;

    public class FraudRadar
    {
        public IEnumerable<FraudResult> Check(IEnumerable<Order> normalizedOrders)
        {
            var orders = normalizedOrders.ToList();

            var fraudResults = new List<FraudResult>();
            var fraudCheckerService = new FraudCheckerService();

            for (int i = 0; i < orders.Count; i++)
            {
                var isLastRow = i == orders.Count - 1;

                if (isLastRow)
                {
                    break;
                }

                var nextId = i + 1;

                var current = orders[i];
                var next = orders[nextId];

                if (fraudCheckerService.ValidateRule(current, next))
                {
                    continue;
                }

                fraudResults.Add(
                    new FraudResult
                    {
                        IsFraudulent = true,
                        OrderId = next.OrderId
                    });
            }

            return fraudResults;
        }
    }
}