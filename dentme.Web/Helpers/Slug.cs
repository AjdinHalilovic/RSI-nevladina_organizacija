using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Globalization;

namespace nevladinaOrg.Web.Helpers
{
    public static class Slug
    {
        public static string Create(string text, SlugOptions options = null)
        {
            if (text == null)
                return null;

            if (options == null)
            {
                options = new SlugOptions();
            }

            string normalized;
            if (options.EarlyTruncate && options.MaximumLength > 0 && text.Length > options.MaximumLength)
            {
                normalized = text.Substring(0, options.MaximumLength).Normalize(NormalizationForm.FormD);
            }
            else
            {
                normalized = text.Normalize(NormalizationForm.FormD);
            }
            var max = options.MaximumLength > 0 ? Math.Min(normalized.Length, options.MaximumLength) : normalized.Length;
            var sb = new StringBuilder(max);
            foreach (var t in normalized)
            {
                var c = t;
                var uc = char.GetUnicodeCategory(c);
                if (options.AllowedUnicodeCategories.Contains(uc) && options.IsAllowed(c))
                {
                    switch (uc)
                    {
                        case UnicodeCategory.UppercaseLetter:
                            if (options.ToLower)
                            {
                                c = options.Culture != null ? char.ToLower(c, options.Culture) : char.ToLowerInvariant(c);
                            }
                            sb.Append(options.Replace(c));
                            break;

                        case UnicodeCategory.LowercaseLetter:
                            if (options.ToUpper)
                            {
                                c = options.Culture != null ? char.ToUpper(c, options.Culture) : char.ToUpperInvariant(c);
                            }
                            sb.Append(options.Replace(c));
                            break;

                        default:
                            sb.Append(options.Replace(c));
                            break;
                    }
                }
                else if (uc == UnicodeCategory.NonSpacingMark)
                {
                }
                else
                {
                    if (options.Separator != null && !EndsWith(sb, options.Separator))
                    {
                        sb.Append(options.Separator);
                    }
                }

                if (options.MaximumLength > 0 && sb.Length >= options.MaximumLength)
                    break;
            }

            var result = sb.ToString();

            if (options.MaximumLength > 0 && result.Length > options.MaximumLength)
            {
                result = result.Substring(0, options.MaximumLength);
            }

            if (!options.CanEndWithSeparator && options.Separator != null && result.EndsWith(options.Separator))
            {
                result = result.Substring(0, result.Length - options.Separator.Length);
            }

            return result.Normalize(NormalizationForm.FormC);
        }

        private static bool EndsWith(StringBuilder sb, string text)
        {
            if (sb.Length < text.Length)
                return false;

            return !text.Where((t, i) => sb[sb.Length - 1 - i] != text[text.Length - 1 - i]).Any();
        }
    }

    public sealed class SlugOptions
    {
        public const int DefaultMaximumLength = 80;
        public const string DefaultSeparator = "-";

        private bool _toLower;
        private bool _toUpper;

        public SlugOptions()
        {
            MaximumLength = DefaultMaximumLength;
            Separator = DefaultSeparator;
            AllowedUnicodeCategories = new List<UnicodeCategory>
            {
                UnicodeCategory.UppercaseLetter,
                UnicodeCategory.LowercaseLetter,
                UnicodeCategory.DecimalDigitNumber
            };
            AllowedRanges = new List<KeyValuePair<short, short>>
            {
                new KeyValuePair<short, short>((short) 'a', (short) 'z'),
                new KeyValuePair<short, short>((short) 'A', (short) 'Z'),
                new KeyValuePair<short, short>((short) '0', (short) '9')
            };
        }

        public IList<UnicodeCategory> AllowedUnicodeCategories { get; }
        public IList<KeyValuePair<short, short>> AllowedRanges { get; }

        public int MaximumLength { get; set; }
        public string Separator { get; set; }
        public CultureInfo Culture { get; set; }
        public bool CanEndWithSeparator { get; set; }
        public bool EarlyTruncate { get; set; }
        public bool ToLower {
            get => _toLower;
            set {
                _toLower = value;
                if (_toLower)
                {
                    _toUpper = false;
                }
            }
        }

        public bool ToUpper {
            get => _toUpper;
            set {
                _toUpper = value;
                if (_toUpper)
                {
                    _toLower = false;
                }
            }
        }

        public bool IsAllowed(char character)
        {
            return AllowedRanges.Any(p => character >= p.Key && character <= p.Value);
        }

        public string Replace(char character)
        {
            return character.ToString();
        }
    }
}
