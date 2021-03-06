﻿// ***********************************************************************
// Project          : MoeLib
// File             : MD5.cs
// Created          : 2015-08-13  3:30 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-09-13  7:05 PM
// ***********************************************************************
// <copyright file="MD5.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.IO;
using System.Security.Cryptography;

namespace MoeLibCore
{
    /// <summary>
    ///     MD5Hash.
    /// </summary>
    public static class MD5Hash
    {
        /// <summary>
        ///     Computes the file hash of the file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>Hash bytes</returns>
        public static byte[] ComputeHashForTheFile(string filePath)
        {
            using (MD5 md5 = MD5.Create())
            {
                using (FileStream stream = File.OpenRead(filePath))
                {
                    return md5.ComputeHash(stream);
                }
            }
        }

        /// <summary>
        ///     Computes the file hash of the file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>Hash string</returns>
        public static string ComputeHashStringForTheFile(string filePath)
        {
            using (MD5 md5 = MD5.Create())
            {
                using (FileStream stream = File.OpenRead(filePath))
                {
                    byte[] bytes = md5.ComputeHash(stream);
                    return BitConverter.ToString(bytes).Remove("-").ToLowerInvariant();
                }
            }
        }

        /// <summary>
        ///     Computes the MD5 hash string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        public static byte[] ComputeMD5Hash(string value)
        {
            if (value == null)
                return null;

            MD5 md5 = MD5.Create();
            return md5.ComputeHash(value.GetBytesOfUTF8());
        }

        /// <summary>
        ///     Computes the MD5 hash string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        public static string ComputeMD5HashString(string value)
        {
            if (value == null)
                return "";

            MD5 md5 = MD5.Create();
            byte[] data = md5.ComputeHash(value.GetBytesOfUTF8());
            return data.Hex().ToLowerInvariant();
        }
    }
}