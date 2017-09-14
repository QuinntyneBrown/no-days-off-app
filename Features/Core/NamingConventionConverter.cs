using System;
using System.Text;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;

namespace NoDaysOffApp.Features.Core
{
    public enum NamingConvention
    {
        PascalCase,
        CamelCase,
        SnakeCase,
        TitleCase,
        AllCaps,
        None
    }

    public class NamingConventionConverter
    {
        public static string Convert(NamingConvention to, string value) => Convert(GetNamingConvention(value), to, value);

        public static string Convert(NamingConvention from, NamingConvention to, string value)
        {
            switch (to) {
                case NamingConvention.CamelCase:
                    value = FirstCharacterUpperAfterADash(value);
                    value = FirstCharacterUpperAfterASpace(value);
                    value = value.Replace("-", "");
                    value = value.Replace(" ", "");
                    return value.First().ToString().ToLower() + value.Substring(1);
                    break;

                case NamingConvention.PascalCase:
                    value = FirstCharacterUpperAfterADash(value);
                    value = FirstCharacterUpperAfterASpace(value);                    
                    value = value.Replace("-", "");
                    value = value.Replace(" ", "");
                    return value.First().ToString().ToUpper() + value.Substring(1);
                    break;

                case NamingConvention.SnakeCase:
                    value = FirstCharacterUpperAfterASpace(value);
                    value = value.Replace(" ", "");
                    return string.Concat(value.Select((x, i) => i > 0 && char.IsUpper(x) ? "-" + x.ToString() : x.ToString())).ToLower();
                    break;

                case NamingConvention.TitleCase:
                    value = FirstCharacterUpperAfterASpace(value);
                    value = FirstCharacterUpperAfterADash(value);
                    value = value.Replace(" ", "");
                    value = InsertSpaceBeforeUpperCase(value);
                    value = value.Replace("-", "");
                    return value.First().ToString().ToUpper() + value.Substring(1);
                    break;

                case NamingConvention.AllCaps:
                    value = Convert(NamingConvention.SnakeCase, value);
                    value = value.Replace("-", "_");
                    value = value.ToUpper();
                    return value;
                    break;
            }

            return value;
        }

        public static string CamelCase(string input)
        {
            return input.First().ToString().ToLower() + input.Substring(1);
        }

        public static string PascalCaseToTitleCase(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "";
            StringBuilder newText = new StringBuilder(input.Length * 2);
            newText.Append(input[0]);
            for (int i = 1; i < input.Length; i++)
            {
                if (char.IsUpper(input[i]) && input[i - 1] != ' ')
                    newText.Append(' ');
                newText.Append(input[i]);
            }
            return newText.ToString();
        }

        public static string SnakeCaseToPascalCase(string input)
        {
            System.Text.StringBuilder resultBuilder = new System.Text.StringBuilder();
            foreach (char c in input)
            {
                if (!Char.IsLetterOrDigit(c))
                {
                    resultBuilder.Append(" ");
                }
                else
                {
                    resultBuilder.Append(c);
                }
            }
            string result = resultBuilder.ToString();
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            result = textInfo.ToTitleCase(result).Replace(" ", String.Empty);
            return result;
        }

        public static NamingConvention GetNamingConvention(string value)
        {
            if (IsNamingConventionType(NamingConvention.CamelCase, value))
                return NamingConvention.CamelCase;

            if (IsNamingConventionType(NamingConvention.PascalCase, value))
                return NamingConvention.PascalCase;

            if (IsNamingConventionType(NamingConvention.SnakeCase, value))
                return NamingConvention.SnakeCase;

            if (IsNamingConventionType(NamingConvention.TitleCase, value))
                return NamingConvention.TitleCase;

            return NamingConvention.None;
        }

        public static bool IsNamingConventionType(NamingConvention namingConvention, string value)
        {
            switch (namingConvention)
            {
                case NamingConvention.CamelCase:
                    return !value.Contains(" ") 
                        && !value.Contains("-")
                        && char.IsLower(value.First());
                    break;

                case NamingConvention.PascalCase:
                    return !value.Contains(" ")
                        && !value.Contains("-")
                        && char.IsUpper(value.First());
                    break;

                case NamingConvention.TitleCase:                   
                    return !value.Contains("-") 
                        && char.IsUpper(value.First());
                    break;

                case NamingConvention.SnakeCase:
                    return !value.Contains(" ") 
                        && !value.Any(c => char.IsUpper(c));
                    break;
            }

            throw new NotImplementedException();
        }

        public static string FirstCharacterUpperAfterASpace(string value)
        {
            List<int> indexesOfTitleCharacter = new List<int>();
            int index = 0;
            foreach (char c in value.ToList())
            {
                if (string.IsNullOrWhiteSpace(c.ToString()))
                    indexesOfTitleCharacter.Add(index + 1);

                index++;
            }

            index = 0;
            var sb = new StringBuilder();
            foreach (char c in value.ToList())
            {
                if (indexesOfTitleCharacter.Any(x => x == index))
                {
                    sb.Append(c.ToString().ToUpper());
                }
                else
                {
                    sb.Append(c);
                }

                index++;
            }

            return sb.ToString();
        }
        public static string FirstCharacterUpperAfterADash(string value)
        {
            List<int> indexesOfTitleCharacter = new List<int>();
            int index = 0;
            foreach (char c in value.ToList())
            {
                if (c.ToString() == "-")
                    indexesOfTitleCharacter.Add(index + 1);
                index++;
            }

            index = 0;
            var sb = new StringBuilder();
            foreach (char c in value.ToList())
            {
                if (indexesOfTitleCharacter.Any(x => x == index))
                {
                    sb.Append(c.ToString().ToUpper());
                }
                else
                {
                    sb.Append(c);
                }

                index++;
            }

            return sb.ToString();
        }

        public static string InsertSpaceBeforeUpperCase(string value)
        {            
            if (string.IsNullOrWhiteSpace(value))
                return "";
            StringBuilder newText = new StringBuilder(value.Length * 2);
            newText.Append(value[0]);
            for (int i = 1; i < value.Length; i++)
            {
                if (char.IsUpper(value[i]) && value[i - 1] != ' ')
                    newText.Append(' ');
                newText.Append(value[i]);
            }
            return newText.ToString();
        }
    }
}
