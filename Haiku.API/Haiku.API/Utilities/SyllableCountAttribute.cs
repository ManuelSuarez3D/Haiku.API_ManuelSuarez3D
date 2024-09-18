using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Haiku.API.Utility
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SyllableCountAttribute : ValidationAttribute
    {
        private readonly int _expectedCount;

        public SyllableCountAttribute(int expectedCount)
        {
            _expectedCount = expectedCount;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string line)
            {
                int syllableCount = BasicCountSyllables(line);
                if (syllableCount != _expectedCount)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }

        public static int BasicCountSyllables(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                return 0;
            }

            line = line.ToLower();
            string[] words = line.Split(new[] { ' ', '\t', '\n', '\r', '.', ',', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
            int totalSyllables = 0;

            foreach (string word in words)
            {
                totalSyllables += BasicCountSyllablesInWord(word);
            }

            return totalSyllables;
        }

        private static int BasicCountSyllablesInWord(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                return 0;
            }

            word = word.ToLower();

            if (word.Length == 1)
            {
                return 1;
            }

            if (word.EndsWith("e") && !Regex.IsMatch(word, @"(le|ue)$"))
            {
                word = word.Substring(0, word.Length - 1);
            }

            Regex vowelGroups = new Regex("[aeiouy]+", RegexOptions.IgnoreCase);
            var matches = vowelGroups.Matches(word);

            int syllableCount = matches.Count;

            if (word.EndsWith("le") && word.Length > 2 && !Regex.IsMatch(word, "[aeiou]le$"))
            {
                syllableCount++;
            }
            return Math.Max(1, syllableCount);
        }

    }
}
