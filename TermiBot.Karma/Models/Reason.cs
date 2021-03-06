using System;
using System.ComponentModel.DataAnnotations;

namespace TermiBot.Karma.Models
{
    public class Reason
    {
        public static string[] Conjunctions = {"for", "because", "due to", "over", "thanks to", "since", "considering"};
        
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Change { get; set; }
        public string Value { get; set; }

        public Reason(string name, int change, string value)
        {
            Name = name;
            Change = change;
            Value = value;
        }
    }
}