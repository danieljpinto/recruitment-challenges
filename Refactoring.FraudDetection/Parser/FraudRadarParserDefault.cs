// <copyright file="FraudRadar.cs" company="Payvision">
// Copyright (c) Payvision. All rights reserved.
// </copyright>

namespace Refactoring.FraudDetection.Parser
{
    using Refactoring.FraudDetection.Model;
    using Refactoring.FraudDetection.Reader;
    using System;
    using System.Collections.Generic;

    public class FraudRadarParserDefault : IFraudRadarParser<Order>
    {
        private readonly IFraudRadarReader _reader;

        public FraudRadarParserDefault(IFraudRadarReader reader)
        {
            _reader = reader ?? throw new ArgumentException("No content was sent. Can't parse an empty file.", nameof(reader));
        }

        public IEnumerable<Order> Parse()
        {
            var orders = new List<Order>();

            foreach (var line in _reader.ReadAll())
            {
                var items = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var order = new Order
                {
                    OrderId = int.Parse(items[0]),
                    DealId = int.Parse(items[1]),
                    Email = items[2].ToLower(),
                    Street = items[3].ToLower(),
                    City = items[4].ToLower(),
                    State = items[5].ToLower(),
                    ZipCode = items[6],
                    CreditCard = items[7]
                };

                orders.Add(order);
            }

            return orders;
        }
    }
}