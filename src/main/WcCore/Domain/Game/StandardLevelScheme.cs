using System;
using System.Collections.Generic;
using System.Text;

namespace WcRunway.Core.Domain.Game
{
    public class StandardLevelScheme
    {
        private const int MAX_LEVEL = 20;
        private const int TOKEN_RANGE = 10;
        private int MAX_TOKENS = TOKEN_RANGE * (TOKEN_RANGE + 1) / 2;

        public int CalculateTokensRemaining(Dictionary<int, int> levelSpread)
        {
            var tokens = 0;

            foreach (KeyValuePair<int, int> entry in levelSpread)
            {
                tokens += CalculateTokensForLevel(entry.Key) * entry.Value;
            }

            return tokens;
        }
        
        public int CalculateTokensForLevel(int level)
        {
            if (level < 10)
            {
                return MAX_TOKENS;
            }
            else if (level >= 10 && level <= 20)
            {
                int tokenEligibleLevels = level - 10;
                return MAX_TOKENS - (tokenEligibleLevels * (tokenEligibleLevels + 1) / 2);
            }
            else
            {
                // warn: wrong scheme being applied
                return 0;
            }
        }
    }
}
