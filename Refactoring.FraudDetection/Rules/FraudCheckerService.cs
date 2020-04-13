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

            // I'm not sure about the business rules, but does not feel right have two identical lines
            _fraudCheckers.Add(new FraudCheckerCurrentAndNextHaveSameDataRule());
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