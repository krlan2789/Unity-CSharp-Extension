using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace LAN.Extension
{
    public static class FormatValidationExtension
    {
        /// <summary>
        /// Convert a phone number to a valid international phone number.
        /// </summary>
        /// <param name="phoneNumber">A valid/invalid phone number</param>
        /// <returns></returns>
        public static string ValidInterPhoneNumber(this string phoneNumber)
        {
            if (phoneNumber.IsEmpty()) return "";

            string validPhoneNumber;

            //  Validasi format nomor telp
            if (phoneNumber[0] == '0')
            {
                validPhoneNumber = "62" + phoneNumber.Substring(1);
            } else if (phoneNumber[0] == '+')
            {
                validPhoneNumber = phoneNumber.Substring(1);
            } else
            {
                validPhoneNumber = phoneNumber;
            }
            return validPhoneNumber;
        }

        /// <summary>
        /// Verify if string is a valid email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsValidEmail(this string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper, RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            } catch (RegexMatchTimeoutException)
            {
                return false;
            } catch (ArgumentException)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            } catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}
