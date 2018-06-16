// ***********************************************************************
// Assembly         : MoeLib
// Author           : Siqi Lu
// Created          : 2015-03-14  10:30 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-03-14  10:30 PM
// ***********************************************************************
// <copyright file="PathUtils.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;

namespace MoeLibCore
{
    /// <summary>
    ///     Utilities methods for working with resource paths
    /// </summary>
    public static class UrlPathUtility
    {
        /// <summary>
        ///     Combines two URL paths
        /// </summary>
        public static Uri CombineUrlPaths(string path1, string path2)
        {
            if (string.IsNullOrEmpty(path2))
                return new Uri(path1);

            if (string.IsNullOrEmpty(path1))
                return new Uri(path2);

            if (path2.StartsWith("http://", StringComparison.Ordinal) || path2.StartsWith("https://", StringComparison.Ordinal))
                return new Uri(path2);

            char ch = path1[path1.Length - 1];

            return ch != '/' ? new Uri(path1.TrimEnd('/') + '/' + path2.TrimStart('/')) : new Uri(path1 + path2);
        }
    }
}