// ***********************************************************************
// Project          : MoeLib
// Author           : Siqi Lu
// Created          : 2015-03-14  8:49 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-30  11:15 PM
// ***********************************************************************
// <copyright file="Null.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;

namespace MoeLibCore
{
    /// <summary>
    ///     Utilities for working with null value.
    /// </summary>
    public static class NullUtility
    {
        /// <summary>
        ///     Does the specified action if the object is not null.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="action">The action.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>TResult.</returns>
        /// <exception cref="ArgumentNullException">action</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public static TResult IfNotNull<TObject, TResult>(this TObject obj, Func<TObject, TResult> action, TResult defaultValue = default(TResult)) where TObject : class
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            return obj != null ? action(obj) : defaultValue;
        }

        /// <summary>
        ///     Does the specified action if the object is not null.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="action">The action.</param>
        /// <exception cref="System.ArgumentNullException">action</exception>
        public static void IfNotNull<TObject>(this TObject obj, Action<TObject> action) where TObject : class
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (obj == null)
                return;
            action(obj);
        }
    }
}