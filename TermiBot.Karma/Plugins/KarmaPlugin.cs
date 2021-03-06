using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Schema;
using Noobot.Core.MessagingPipeline.Request;
using Noobot.Core.MessagingPipeline.Response;
using Noobot.Core.Plugins;
using TermiBot.Karma.Models;

namespace TermiBot.Karma.Plugins
{
    public class KarmaPlugin : IPlugin
    {
        public static string IncomingMessageRegex = @"([^\`\s]{2,})[^\`\s-\+](--|\+\+)(?!\b)";
        private const string BacktickQuoteRegex = @"\`.*\`";
        private string positiveKarmaOperator = "++";
        
        public void Start()
        {
            
        }

        public void Stop()
        {
            
        }
        
        public ChangeRequest ParseKarmaChange(string matchedText)
        {
            int itemLength = matchedText.Length - 2;
            var matchedItem = matchedText.Substring(0, itemLength);
            var karmaOperator = matchedText.Substring(itemLength, 2);
            var changeAmount = karmaOperator.Equals(positiveKarmaOperator) ? 1 : -1;
            
            return new ChangeRequest(matchedItem, changeAmount);
        }

        public IList<Match> GetMessageMatches(string message)
        {
            var inlineCodeMatches = Regex.Matches(message, BacktickQuoteRegex);
            var karmaPhraseMatches = Regex.Matches(message, IncomingMessageRegex);
            return karmaPhraseMatches.Where(x => !IsInsideInlineCode(x, inlineCodeMatches)).ToList();
        }

        private bool IsInsideInlineCode(Match match, MatchCollection inlineCodeMatches)
        {
            foreach (Match inlineCodeMatch in inlineCodeMatches)
            {
                if (RangeContainsRange(inlineCodeMatch.Index, inlineCodeMatch.Length,
                    match.Index, match.Length)) return true;
            }

            return false;
        }

        private bool RangeContainsRange(int aIndex, int aLength, int bIndex, int bLength)
        {
            int aEnd = aIndex + aLength;
            int bEnd = bIndex + bLength;

            if (bIndex > aIndex && bIndex > aEnd) return false;
            if (bIndex < aIndex && bEnd < aIndex) return false;
            return true;
        }
        
        public string GenerateCurrentKarmaMessage(ChangeRequest changeRequest,
            int currentKarma)
        {
            string emoji = changeRequest.Amount > 0 ? ":upboat:" : ":downboat:";
            var karmaItemName = changeRequest.Name.Replace("_"," ");
            
            return $"{emoji} {karmaItemName}: {currentKarma}";
        }
    }
}