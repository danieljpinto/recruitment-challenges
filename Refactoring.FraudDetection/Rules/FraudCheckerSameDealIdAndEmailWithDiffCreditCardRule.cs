// <copyright file="FraudRadar.cs" company="Payvision">
// Copyright (c) Payvision. All rights reserved.
// </copyright>

namespace Refactoring.FraudDetection.Rules
{
    using Refactoring.FraudDetection.Model;

    public class FraudCheckerSameDealIdAndEmailWithDiffCreditCardRule : IFraudChecker<Order>
    {
        public bool IsFraudulent(Order current, Order next)
        {
            return current.DealId == next.DealId
                && current.Email == next.Email
                && current.CreditCard != next.CreditCard;
        }
    }
}