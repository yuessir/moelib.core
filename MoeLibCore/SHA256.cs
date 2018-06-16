// ***********************************************************************
// Assembly         : MoeLib
// Author           : Siqi Lu
// Created          : 2015-03-14  10:48 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-03-14  10:54 PM
// ***********************************************************************
// <copyright file="SHA256.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;

namespace MoeLibCore
{
    /// <summary>
    ///     Class Sha256Utility.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Sha")]
    public static class Sha256Utility
    {
        /// <summary>
        ///     Hashes the specified payload.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <param name="salt">The salt.</param>
        /// <returns>System.String.</returns>
        public static string Hash(string payload, string salt)
        {
            string stringToHash = payload + salt;
            SHA256 sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(stringToHash.GetBytesOfUTF8());
            StringBuilder hashString = new StringBuilder();
            foreach (byte b in hashBytes)
                hashString.Append(b.ToString("x2"));
            return hashString.ToString();
        }
    }
}