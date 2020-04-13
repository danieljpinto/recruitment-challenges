using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refactoring.FraudDetection.Model;
using Refactoring.FraudDetection.Rules;

namespace Refactoring.FraudDetection.Tests.Rules
{
    [TestClass]
    public class FraudCheckerSameDealIdAndEmailWithDiffCreditCardRuleTests
    {
        [TestMethod]
        public void IsFraudulent_SameDataWithDiffCreditCard_ThenIsFraudulent()
        {
            var current = new Order { DealId = 1, State = "XX", ZipCode = "11111", Street = "Street Something", City = "City", CreditCard = "1111222233334444", Email = "email@email.com", OrderId = 1 };
            var next = new Order { DealId = 1, State = "XX", ZipCode = "11111", Street = "Street Something", City = "City", CreditCard = "1111222233334445", Email = "email@email.com", OrderId = 1 };

            var rule = new FraudCheckerSameDealIdAndEmailWithDiffCreditCardRule().IsFraudulent(current, next);

            Assert.IsTrue(rule);
        }

        [TestMethod]
        public void IsFraudulent_SameData_ThenIsNotFraudulent()
        {
            var current = new Order { DealId = 1, State = "XX", ZipCode = "11111", Street = "Street Something", City = "City", CreditCard = "1111222233334444", Email = "email@email.com", OrderId = 1 };
            var next = new Order { DealId = 1, State = "XX", ZipCode = "11111", Street = "Street Something", City = "City", CreditCard = "1111222233334444", Email = "email@email.com", OrderId = 1 };

            var rule = new FraudCheckerSameDealIdAndEmailWithDiffCreditCardRule().IsFraudulent(current, next);

            Assert.IsFalse(rule);
        }
    }
}
