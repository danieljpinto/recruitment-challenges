// <copyright file="FraudRadar.cs" company="Payvision">
// Copyright (c) Payvision. All rights reserved.
// </copyright>

namespace Refactoring.FraudDetection
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public interface IFraudRadarReader
    {
        IEnumerable<string> ReadAllLines();
    }

    public class FraudRadarReader : IFraudRadarReader
    {
        private string _filePath;

        public FraudRadarReader(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new ArgumentException($"The provided path was not found ({filePath}).");
            }

            _filePath = filePath;
        }

        public IEnumerable<string> ReadAllLines()
        {
            if (string.IsNullOrEmpty(_filePath))
            {
                throw new Exception("File was not Open");
            }

            var result = new List<string>();

            using (var file = new StreamReader(_filePath))
            {
                string ln;

                while ((ln = file.ReadLine()) != null)
                {
                    result.Add(ln);
                }

                file.Close();
            }

            return result;
        }
    }

    public interface IFraudRadarParser<T>
    {
        IEnumerable<T> Parse();
    }

    public class FraudRadarParserDefault : IFraudRadarParser<Order>
    {
        private readonly IEnumerable<string> _content;

        public FraudRadarParserDefault(IEnumerable<string> content)
        {
            _content = content ?? throw new ArgumentException("No content was sent. Can't parse an empty file.", nameof(content));
        }

        public IEnumerable<Order> Parse()
        {
            var orders = new List<Order>();

            foreach (var line in _content)
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

    public class FraudRadar
    {
        public IEnumerable<FraudResult> Check(IFraudRadarReader reader)
        {
            var fraudResults = new List<FraudResult>();

            var lines = reader.ReadAllLines();

            var parser = new FraudRadarParserDefault(lines);

            var orders = parser.Parse();

            // NORMALIZE
            var n1 = new FraudRadarEmailNormalizer();
            var n2 = new FraudRadarStreetNormalizer();
            var n3 = new FraudRadarStateNormalizer();

            n1.SetNextNormalizer(n2);
            n2.SetNextNormalizer(n3);

            foreach (var order in orders)
            {
                n1.Normalize(order);
            }

            // CHECK FRAUD
            var nOrders = orders.ToList();

            for (int i = 0; i < nOrders.Count; i++)
            {
                var nextId = i == nOrders.Count - 1 ? i : i + 1;

                var current = nOrders[i];
                var next = nOrders[nextId];

                bool isFraudulent;

                isFraudulent = false;

                if (current.DealId == next.DealId
                    && current.Email == next.Email
                    && current.CreditCard != next.CreditCard)
                {
                    isFraudulent = true;
                }

                if (current.DealId == next.DealId
                    && current.State == next.State
                    && current.ZipCode == next.ZipCode
                    && current.Street == next.Street
                    && current.City == next.City
                    && current.CreditCard != next.CreditCard)
                {
                    isFraudulent = true;
                }

                if (isFraudulent)
                {
                    fraudResults.Add(new FraudResult { IsFraudulent = true, OrderId = next.OrderId });
                }
            }

            return fraudResults;
        }
    }

    public class FraudResult
    {
        public int OrderId { get; set; }

        public bool IsFraudulent { get; set; }
    }

    public class Order
    {
        public int OrderId { get; set; }

        public int DealId { get; set; }

        public string Email { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public string CreditCard { get; set; }
    }

    public abstract class FraudRadarNormalizer<T>
    {
        protected FraudRadarNormalizer<T> next;

        public void SetNextNormalizer(FraudRadarNormalizer<T> next)
        {
            this.next = next;
        }

        public abstract void Normalize(T item);
    }

    public class FraudRadarEmailNormalizer : FraudRadarNormalizer<Order>
    {
        public override void Normalize(Order order)
        {
            var aux = order.Email.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);

            var atIndex = aux[0].IndexOf("+", StringComparison.Ordinal);

            aux[0] = atIndex < 0 ? aux[0].Replace(".", "") : aux[0].Replace(".", "").Remove(atIndex);

            order.Email = string.Join("@", new string[] { aux[0], aux[1] });

            if (base.next != null)
            {
                base.next.Normalize(order);
            }
        }
    }

    public class FraudRadarStreetNormalizer : FraudRadarNormalizer<Order>
    {
        public override void Normalize(Order order)
        {
            order.Street = order.Street.Replace("st.", "street").Replace("rd.", "road");

            if (base.next != null)
            {
                base.next.Normalize(order);
            }
        }
    }

    public class FraudRadarStateNormalizer : FraudRadarNormalizer<Order>
    {
        public override void Normalize(Order order)
        {
            order.State = order.State.Replace("il", "illinois").Replace("ca", "california").Replace("ny", "new york");

            if (base.next != null)
            {
                base.next.Normalize(order);
            }
        }
    }
}