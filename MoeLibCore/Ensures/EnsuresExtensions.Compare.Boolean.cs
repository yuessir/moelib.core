// ***********************************************************************
// Assembly         : MoeEnsure
// Author           : Siqi Lu
// Created          : 2015-03-15  2:52 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-03-17  12:29 AM
// ***********************************************************************
// <copyright file="EnsuresExtensions.Compare.Boolean.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;

namespace MoeLibCore
{
    /// <summary>
    ///     Extensions of Ensures utility for the <see cref="System.Boolean" />.
    /// </summary>
    public static partial class EnsuresExtensions
    {
        /// <summary>
        ///     Checks whether the given value is <b>false</b>.
        /// </summary>
        /// <param name="ensures">The <see cref="Ensures{T}" /> that holds the value that has to be test/ensure.</param>
        /// <returns>The specified <paramref name="ensures" /> instance.</returns>
        public static Ensures<bool> IsFalse(this Ensures<bool> ensures)
        {
            if (ensures == null)
            {
                throw new ArgumentNullException(nameof(ensures));
            }

            return ensures.That(v => v == false);
        }

        /// <summary>
        ///     Checks whether the given value is <b>false</b>.
        /// </summary>
        /// <param name="ensures">The <see cref="Ensures{T}" /> that holds the value that has to be test/ensure.</param>
        /// <returns>The specified <paramref name="ensures" /> instance.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public static Ensures<bool?> IsFalse(this Ensures<bool?> ensures)
        {
            if (ensures == null)
            {
                throw new ArgumentNullException(nameof(ensures));
            }

            return ensures.That(v => v.HasValue && v.Value == false);
        }

        /// <summary>
        ///     Checks whether the given value is <b>true</b>.
        /// </summary>
        /// <param name="ensures">The <see cref="Ensures{T}" /> that holds the value that has to be test/ensure.</param>
        /// <returns>The specified <paramref name="ensures" /> instance.</returns>
        public static Ensures<bool> IsTrue(this Ensures<bool> ensures)
        {
            if (ensures == null)
            {
                throw new ArgumentNullException(nameof(ensures));
            }

            return ensures.That(v => v);
        }

        /// <summary>
        ///     Checks whether the given value is <b>true</b>.
        /// </summary>
        /// <param name="ensures">The <see cref="Ensures{T}" /> that holds the value that has to be test/ensure.</param>
        /// <returns>The specified <paramref name="ensures" /> instance.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public static Ensures<bool?> IsTrue(this Ensures<bool?> ensures)
        {
            if (ensures == null)
            {
                throw new ArgumentNullException(nameof(ensures));
            }

            return ensures.That(v => v.HasValue && v.Value);
        }
    }
}