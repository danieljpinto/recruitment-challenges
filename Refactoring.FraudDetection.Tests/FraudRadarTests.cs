// <copyright file="FraudRadarTests.cs" company="Payvision">
// Copyright (c) Payvision. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NSubstitute.ReturnsExtensions;
using Refactoring.FraudDetection.Model;
using Refactoring.FraudDetection.Normalizer;
using Refactoring.FraudDetection.Parser;
using Refactoring.FraudDetection.Reader;

namespace Refactoring.FraudDetection.Tests
{
    [TestClass]
    public class FraudRadarTests
    {
        [TestMethod]
        public void CheckFraud_ZeroLineFile_NoFraudExpected()
        {
            var readerMock = new Mock<IFraudRadarReader>();
            readerMock.Setup(_ => _.ReadAll()).Returns(new List<string>());

            var reader = readerMock.Object;
            var orders = new FraudRadarParserDefault(reader).Parse();
            var normalizedOrders = new FraudRadarOrderNormalizer().Normalize(orders);

            var result = new FraudRadar().Check(normalizedOrders).ToList();

            result.Should().NotBeNull("The result should not be null.");
            result.Should().HaveCount(0, "The result should not contains fraudulent lines");
        }

        [TestMethod]
        public void CheckFraud_OneLineFile_NoFraudExpected()
        {
            var readerMock = new Mock<IFraudRadarReader>();
            readerMock.Setup(_ => _.ReadAll()).Returns(new List<string> { "1,1,bugs@bunny.com,123 Sesame St.,New York,NY,10011,12345689010" });

            var reader = readerMock.Object;
            var orders = new FraudRadarParserDefault(reader).Parse();
            var normalizedOrders = new FraudRadarOrderNormalizer().Normalize(orders);

            var result = new FraudRadar().Check(normalizedOrders).ToList();

            result.Should().NotBeNull("The result should not be null.");
            result.Should().HaveCount(0, "The result should not contains fraudulent lines");
        }

        [TestMethod]
        public void CheckFraud_TwoLines_SecondLineFraudulent()
        {
            var readerMock = new Mock<IFraudRadarReader>();
            readerMock.Setup(_ => _.ReadAll()).Returns(
                new List<string>
                {
                    "1,1,bugs@bunny.com,123 Sesame St.,New York,NY,10011,12345689010",
                    "2,1,bugs@bunny.com,123 Sesame St.,New York,NY,10011,12345689011"
                });

            var reader = readerMock.Object;
            var orders = new FraudRadarParserDefault(reader).Parse();
            var normalizedOrders = new FraudRadarOrderNormalizer().Normalize(orders);

            var result = new FraudRadar().Check(normalizedOrders).ToList();

            result.Should().NotBeNull("The result should not be null.");
            result.Should().HaveCount(1, "The result should contains the number of lines of the file");
            result.First().IsFraudulent.Should().BeTrue("The first line is not fraudulent");
            result.First().OrderId.Should().Be(2, "The first line is not fraudulent");
        }

        [TestMethod]
        public void CheckFraud_TwoIdenticalLines_SecondLineFraudulent()
        {
            var readerMock = new Mock<IFraudRadarReader>();
            readerMock.Setup(_ => _.ReadAll()).Returns(
                new List<string>
                {
                    "1,1,bugs@bunny.com,123 Sesame St.,New York,NY,10011,12345689010",
                    "1,1,bugs@bunny.com,123 Sesame St.,New York,NY,10011,12345689010"
                });

            var reader = readerMock.Object;
            var orders = new FraudRadarParserDefault(reader).Parse();
            var normalizedOrders = new FraudRadarOrderNormalizer().Normalize(orders);

            var result = new FraudRadar().Check(normalizedOrders).ToList();

            result.Should().NotBeNull("The result should not be null.");
            result.Should().HaveCount(1, "The result should contains the number of lines of the file");
            result.First().IsFraudulent.Should().BeTrue("The first line is not fraudulent");
            result.First().OrderId.Should().Be(1, "The first line is not fraudulent");
        }

        [TestMethod]
        public void CheckFraud_ThreeLines_SecondLineFraudulent()
        {
            var readerMock = new Mock<IFraudRadarReader>();
            readerMock.Setup(_ => _.ReadAll()).Returns(
                new List<string>
                {
                    "1,1,bugs@bunny.com,123 Sesame St.,New York,NY,10011,12345689010",
                    "2,1,bugs@bunny.com,123 Sesame St.,New York,NY,10011,12345689011",
                    "3,2,roger@rabbit.com,1234 Not Sesame St.,Colorado,CL,10012,12345689012"
                });

            var reader = readerMock.Object;
            var orders = new FraudRadarParserDefault(reader).Parse();
            var normalizedOrders = new FraudRadarOrderNormalizer().Normalize(orders);

            var result = new FraudRadar().Check(normalizedOrders).ToList();

            result.Should().NotBeNull("The result should not be null.");
            result.Should().HaveCount(1, "The result should contains the number of lines of the file");
            result.First().IsFraudulent.Should().BeTrue("The first line is not fraudulent");
            result.First().OrderId.Should().Be(2, "The first line is not fraudulent");
        }

        [TestMethod]
        public void CheckFraud_FourLines_MoreThanOneFraudulent()
        {
            var readerMock = new Mock<IFraudRadarReader>();
            readerMock.Setup(_ => _.ReadAll()).Returns(
                new List<string>
                {
                    "1,1,bugs@bunny.com,123 Sesame St.,New York,NY,10011,12345689010",
                    "2,1,bugs@bunny.com,123 Sesame St.,New York,NY,10011,12345689011",
                    "3,2,roger@rabbit.com,1234 Not Sesame St.,Colorado,CL,10012,12345689012",
                    "4,2,roger@rabbit.com,1234 Not Sesame St.,Colorado,CL,10012,12345689014"
                });

            var reader = readerMock.Object;
            var orders = new FraudRadarParserDefault(reader).Parse();
            var normalizedOrders = new FraudRadarOrderNormalizer().Normalize(orders);

            var result = new FraudRadar().Check(normalizedOrders).ToList();

            result.Should().NotBeNull("The result should not be null.");
            result.Should().HaveCount(2, "The result should contains the number of lines of the file");
        }
    }
}