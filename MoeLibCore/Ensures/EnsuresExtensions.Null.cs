// ***********************************************************************
// Project          : MoeLib
// Author           : Siqi Lu
// Created          : 2015-05-30  11:32 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-30  11:34 PM
// ***********************************************************************
// <copyright file="EnsuresExtensions.Null.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Diagnostics.CodeAnalysis;

namespace MoeLibCore
{
    /// <summary>
    ///     Extensions of Ensures utility for the null value.
    /// </summary>
    public static partial class EnsuresExtensions
    {
        /// <summary>
        ///     Checks whether the given value is not null.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="Ensures{T}">Value</see> of the specified <paramref name="ensures" />.</typeparam>
        /// <param name="ensures">The <see cref="Ensures{T}" /> that holds the value that has to be test/ensure.</param>
        /// <returns>The specified <paramref name="ensures" /> instance.</returns>
        public static Ensures<T> IsNotNull<T>(this Ensures<T> ensures) where T : class
        {
            if (ensures == null)
            {
                throw new ArgumentNullException(nameof(ensures));
            }

            return ensures.That(v => v != null);
        }

        /// <summary>
        ///     Checks whether the given value is not null.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="Ensures{T}">Value</see> of the specified <paramref name="ensures" />.</typeparam>
        /// <param name="ensures">The <see cref="Ensures{T}" /> that holds the value that has to be test/ensure.</param>
        /// <returns>The specified <paramref name="ensures" /> instance.</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public static Ensures<T?> IsNotNull<T>(this Ensures<T?> ensures) where T : struct
        {
            if (ensures == null)
            {
                throw new ArgumentNullException(nameof(ensures));
            }

            return ensures.That(v => v.HasValue);
        }

        /// <summary>
        ///     Checks whether the given value is null.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="Ensures{T}">Value</see> of the specified <paramref name="ensures" />.</typeparam>
        /// <param name="ensures">The <see cref="Ensures{T}" /> that holds the value that has to be test/ensure.</param>
        /// <returns>The specified <paramref name="ensures" /> instance.</returns>
        public static Ensures<T> IsNull<T>(this Ensures<T> ensures) where T : class
        {
            if (ensures == null)
            {
                throw new ArgumentNullException(nameof(ensures));
            }

            return ensures.That(v => v == null);
        }

        /// <summary>
        ///     Checks whether the given value is null. An exception is thrown otherwise.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="Ensures{T}">Value</see> of the specified <paramref name="ensures" />.</typeparam>
        /// <param name="ensures">The <see cref="Ensures{T}" /> that holds the value that has to be test/ensure.</param>
        /// <returns>The specified <paramref name="ensures" /> instance.</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public static Ensures<T?> IsNull<T>(this Ensures<T?> ensures)
            where T : struct
        {
            if (ensures == null)
            {
                throw new ArgumentNullException(nameof(ensures));
            }

            return ensures.That(v => !v.HasValue);
        }
    }
}