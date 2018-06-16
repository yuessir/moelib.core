// ***********************************************************************
// Assembly         : MoeEnsure
// Author           : Siqi Lu
// Created          : 2015-03-15  2:52 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-03-22  11:27 PM
// ***********************************************************************
// <copyright file="It.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

namespace MoeLibCore
{
    /// <summary>
    ///     Use <see cref="It" /> class to construct <see cref="Ensures{T}" /> instance.
    /// </summary>
    public static class It
    {
        /// <summary>
        ///     Construct a <see cref="Ensures{T}" /> instance for the value.
        /// </summary>
        /// <typeparam name="T">Type of the value to test/ensure.</typeparam>
        /// <param name="value">The value to test/ensure of the <see cref="Ensures{T}" /> instance.</param>
        /// <returns>The specified <see cref="Ensures{T}" /> instance.</returns>
        public static Ensures<T> Ensures<T>(T value)
        {
            return new Ensures<T>(value);
        }

        /// <summary>
        ///     Construct a <see cref="Ensures{T}" /> instance for an object.
        /// </summary>
        /// <returns>The specified <see cref="Ensures{T}" /> instance for an object.</returns>
        public static Ensures<object> Ensures()
        {
            return new Ensures<object>(new object());
        }
    }
}