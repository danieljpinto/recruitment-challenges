// <copyright file="FraudRadar.cs" company="Payvision">
// Copyright (c) Payvision. All rights reserved.
// </copyright>

using Refactoring.FraudDetection.Model;
using System.Collections.Generic;

namespace Refactoring.FraudDetection.Rules
{
    public class FraudCheckerService : IFraudCheckerService<Order>
    {
        private readonly List<IFraudChecker<Order>> _fraudCheckers = new List<IFraudChecker<Order>>();

        public FraudCheckerService()
        {
            _fraudCheckers.Add(new FraudCheckerSameDealIdAndAddressWithDiffCreditCardRule());
            _fraudCheckers.Add(new FraudCheckerSameDealIdAndEmailWithDiffCreditCardRule());
        }

        public bool ValidateRule(Order current, Order next)
        {
            foreach (var checker in _fraudCheckers)
            {
                if (checker.IsFraudulent(current, next))
                {
                    return false;
                }
            }

            return true;
        }
    }
}