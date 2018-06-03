using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AuditLogApp.Models.Shared
{
    public class ApiError
    {
        private ModelStateDictionary modelState;

        [Obsolete("Serialization only")]
        public ApiError() { }

        public ApiError(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetMessageForStatusCode(statusCode);
        }

        public ApiError(int statusCode, ModelStateDictionary modelState)
        {
            StatusCode = statusCode;
            Message = GetMessageForStatusCode(statusCode);

            var errors = modelState.Select(p => new { key = CorrectCase(p.Key), errors = String.Join(", ", p.Value.Errors.Select(e => GetErrorMessage(e)).Distinct()) });
            Details = errors.ToDictionary(e => e.key, e => e.errors);
        }

        private string CorrectCase(string key)
        {
            // the documented solution to surface fields cased in camelCase does not work for arrays, so we'll do the work for them
            return String.Join(".", key.Split('.').Select(s => ToCamelCase(s)));
        }

        /// <summary>
        /// Copied from Netwonsoft.Json because the StringUtils class is marked internal: 
        /// https://github.com/JamesNK/Newtonsoft.Json/blob/master/Src/Newtonsoft.Json/Utilities/StringUtils.cs#L149
        /// </summary>
        public static string ToCamelCase(string s)
        {
            if (string.IsNullOrEmpty(s) || !char.IsUpper(s[0]))
            {
                return s;
            }

            char[] chars = s.ToCharArray();

            for (int i = 0; i < chars.Length; i++)
            {
                if (i == 1 && !char.IsUpper(chars[i]))
                {
                    break;
                }

                bool hasNext = (i + 1 < chars.Length);
                if (i > 0 && hasNext && !char.IsUpper(chars[i + 1]))
                {
                    // if the next character is a space, which is not considered uppercase 
                    // (otherwise we wouldn't be here...)
                    // we want to ensure that the following:
                    // 'FOO bar' is rewritten as 'foo bar', and not as 'foO bar'
                    // The code was written in such a way that the first word in uppercase
                    // ends when if finds an uppercase letter followed by a lowercase letter.
                    // now a ' ' (space, (char)32) is considered not upper
                    // but in that case we still want our current character to become lowercase
                    if (char.IsSeparator(chars[i + 1]))
                    {
                        chars[i] = ToLower(chars[i]);
                    }

                    break;
                }

                chars[i] = ToLower(chars[i]);
            }

            return new string(chars);
        }

        private static char ToLower(char c)
        {
            return char.ToLower(c, CultureInfo.InvariantCulture);
        }

        private string GetErrorMessage(ModelError e)
        {
            if (!string.IsNullOrEmpty(e.ErrorMessage))
            {
                return e.ErrorMessage;
            }

            if (e.Exception is Newtonsoft.Json.JsonException || e.Exception is Newtonsoft.Json.JsonReaderException)
            {
                return "Invalid value/type or JSON syntax in error";
            }

            return "Unknown error";
        }

        private string GetMessageForStatusCode(int statusCode)
        {
            switch (statusCode)
            {
                case 400:
                    return "Bad Request";
                case 404:
                    return "Resource Not Found";
                case 500:
                    return "Internal Server Error";
                default:
                    return String.Empty;
            }
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
        public Dictionary<string, string> Details { get; set; }
    }
}
