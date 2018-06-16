// ***********************************************************************
// Project          : MoeLib
// Author           : Siqi Lu
// Created          : 2015-03-14  10:49 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-19  8:59 PM
// ***********************************************************************
// <copyright file="Crypto.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Diagnostics.CodeAnalysis;

namespace MoeLibCore
{
    /// <summary>
    ///     Class Crypto.
    /// </summary>
    public static class Crypto
    {
        /// <summary>
        ///     Gets the encrypted string.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <param name="salt">The salt.</param>
        /// <returns>System.String.</returns>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "PBKDF")]
        public static string PBKDF2(string payload, string salt)
        {
            return PBKDF2Utility.Hash(payload, salt);
        }

        /// <summary>
        ///     Gets the encrypted string.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <param name="salt">The salt.</param>
        /// <returns>System.String.</returns>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Sha")]
        public static string Sha256(string payload, string salt)
        {
            return Sha256Utility.Hash(payload, salt);
        }
    }
}