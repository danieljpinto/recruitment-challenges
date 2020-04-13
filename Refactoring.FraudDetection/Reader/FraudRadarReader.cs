// <copyright file="FraudRadar.cs" company="Payvision">
// Copyright (c) Payvision. All rights reserved.
// </copyright>

namespace Refactoring.FraudDetection.Reader
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class FraudRadarReader : IFraudRadarReader
    {
        private readonly string _filePath;

        public FraudRadarReader(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new ArgumentException($"The provided path was not found ({filePath}).");
            }

            _filePath = filePath;
        }

        public IEnumerable<string> ReadAll()
        {
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
}