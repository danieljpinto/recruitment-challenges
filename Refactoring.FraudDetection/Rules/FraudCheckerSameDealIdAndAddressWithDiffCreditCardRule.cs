// <copyright file="FraudRadar.cs" company="Payvision">
// Copyright (c) Payvision. All rights reserved.
// </copyright>

namespace Refactoring.FraudDetection.Rules
{
    using Refactoring.FraudDetection.Model;

    public class FraudCheckerSameDealIdAndAddressWithDiffCreditCardRule : IFraudChecker<Order>
    {
        public bool IsFraudulent(Order current, Order next)
        {
            return current.DealId == next.DealId
                    && current.State == next.State
                    && current.ZipCode == next.ZipCode
                    && current.Street == next.Street
                    && current.City == next.City
                    && current.CreditCard != next.CreditCard;
        }
    }
}