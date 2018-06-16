// ***********************************************************************
// Assembly         : MoeLib
// Author           : Siqi Lu
// Created          : 2015-03-14  8:43 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-03-14  10:23 PM
// ***********************************************************************
// <copyright file="Enum.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.ComponentModel;
using System.Reflection;

namespace MoeLibCore
{
    /// <summary>
    ///     Extensions of <see cref="System.Enum" /> types.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        ///     Get the descriptions of enum value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        public static string Description(this Enum value)
        {
            return EnumUtility.GetDescription(value);
        }
    }

    /// <summary>
    ///     Utilities for working with <see cref="System.Enum" /> types.
    /// </summary>
    public static class EnumUtility
    {
        /// <summary>
        ///     Gets the description.
        /// </summary>
        /// <param name="value">The enum object.</param>
        /// <returns>System.String.</returns>
        public static string GetDescription(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }

        /// <summary>
        ///     Converts the int representation of the name or numeric value of one or
        ///     more enumerated constants to an equivalent enumerated object.
        /// </summary>
        /// <typeparam name="TEnum">An enumeration type.</typeparam>
        /// <param name="value">An int containing the value to convert.</param>
        /// <returns>An object of type <typeparamref name="TEnum" /> whose value is represented by <paramref name="value" />.</returns>
        public static TEnum ParseFromInt<TEnum>(int value)
        {
            return (TEnum)Enum.ToObject(typeof(TEnum), value);
        }

        /// <summary>
        ///     Converts the string representation of the name or numeric value of one or
        ///     more enumerated constants to an equivalent enumerated object.
        /// </summary>
        /// <typeparam name="TEnum">An enumeration type.</typeparam>
        /// <param name="value">A string containing the name or value to convert.</param>
        /// <returns>An object of type <typeparamref name="TEnum" /> whose value is represented by <paramref name="value" />.</returns>
        public static TEnum ParseFromString<TEnum>(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return default(TEnum);
            }
            return (TEnum)Enum.Parse(typeof(TEnum), value);
        }
    }
}