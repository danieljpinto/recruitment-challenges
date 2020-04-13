using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refactoring.FraudDetection.Model;
using Refactoring.FraudDetection.Normalizer.Types;

namespace Refactoring.FraudDetection.Tests.Normalizer.Types
{
    [TestClass]
    public class NormalizerTests
    {
        [DataTestMethod]
        [DataRow("ro+ger@rabbit.com", true)]
        [DataRow("ro.ger@rabbit.com", true)]
        [DataRow("bugs@bunny.com", false)]
        [DataRow("ro.+ger@rabbit.com", true)] // ??? Not sure about this rule
        [DataRow("roger@rabbit..com", false)] // ??? Not sure about this rule
        [DataRow("roger @rabbit.com", false)] // ??? Not sure about this rule
        [DataRow("roger@rabb it.com", false)] // ??? Not sure about this rule
        [DataRow(null, false)]
        public void FraudRadarEmailNormalize_WhenValueProvided_ThenCompareResultWithIsModifiedAttr(string email, bool isModified)
        {
            var original = email;

            var order = new Order { Email = email };

            new FraudRadarEmailNormalizer().Normalize(order);

            Assert.IsNotNull(order, "The result should not be null.");
            Assert.IsTrue((original == order.Email) != isModified);
        }

        [DataTestMethod]
        [DataRow("il", true)]
        [DataRow("ny", true)]
        [DataRow("ca", true)]
        [DataRow("xx", false)]
        [DataRow(null, false)]
        public void FraudRadarStateNormalizer_WithValueProvided_ThenCompareResultWithIsModifiedAttr(string state, bool isModified)
        {
            var original = state;

            var order = new Order { State = state };

            new FraudRadarStateNormalizer().Normalize(order);

            Assert.IsNotNull(order, "The result should not be null.");
            Assert.IsTrue((original == order.State) != isModified);
        }

        [DataTestMethod]
        [DataRow("st.", true)]
        [DataRow("rd.", true)]
        [DataRow("xx", false)]
        [DataRow(null, false)]
        public void FraudRadarStreetNormalizer_WithValueProvided_ThenCompareResultWithIsModifiedAttr(string street, bool isModified)
        {
            var original = street;

            var order = new Order { Street = street };

            new FraudRadarStreetNormalizer().Normalize(order);

            Assert.IsNotNull(order, "The result should not be null.");
            Assert.IsTrue((original == order.Street) != isModified);
        }
    }
}
