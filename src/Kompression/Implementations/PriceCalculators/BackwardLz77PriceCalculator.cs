﻿using Kompression.PatternMatch;

namespace Kompression.Implementations.PriceCalculators
{
    class BackwardLz77PriceCalculator:IPriceCalculator
    {
        public int CalculateLiteralPrice(int value)
        {
            return 9;
        }

        public int CalculateMatchPrice(Match match)
        {
            return 17;
        }
    }
}