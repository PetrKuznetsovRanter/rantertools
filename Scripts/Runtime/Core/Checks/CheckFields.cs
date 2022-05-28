using System;
using System.Text.RegularExpressions;

namespace RanterTools.Checks
{
    /// <summary>
    ///     Check for some fields.
    /// </summary>
    public static class CheckFields
    {
        /// <summary>
        ///     Check email field.
        /// </summary>
        /// <param name="email">Email.</param>
        /// <param name="correct">Callback for correct email.</param>
        /// <param name="wrong">Callback for wrong email.</param>
        public static void CheckEmail(string email, Action correct, Action wrong)
        {
            if (!Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
            {
                if (wrong != null)
                    wrong();
            }
            else
            {
                if (correct != null)
                    correct();
            }
        }

        /// <summary>
        ///     Check password field.
        /// </summary>
        /// <param name="password">Password.</param>
        /// <param name="correct">Callback for correct password.</param>
        /// <param name="wrong">Callback for wrong password.</param>
        public static void CheckPassword(string password, Action correct, Action wrong)
        {
            if (!Regex.IsMatch(password, @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$"))
            {
                if (wrong != null)
                    wrong();
            }
            else
            {
                if (correct != null)
                    correct();
            }
        }
    }
}