// <copyright file="FraudRadar.cs" company="Payvision">
// Copyright (c) Payvision. All rights reserved.
// </copyright>

namespace Refactoring.FraudDetection.Normalizer
{
    public abstract class FraudRadarNormalizer<T>
    {
        protected FraudRadarNormalizer<T> next;

        public void SetNextNormalizer(FraudRadarNormalizer<T> next)
        {
            this.next = next;
        }

        public abstract void Normalize(T item);
    }
}