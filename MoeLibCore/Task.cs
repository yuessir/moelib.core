// ***********************************************************************
// Project          : MoeLib
// File             : Task.cs
// Created          : 2015-08-13  3:30 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-09-13  7:05 PM
// ***********************************************************************
// <copyright file="Task.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;

namespace MoeLibCore
{
    /// <summary>
    ///     TaskEx.
    /// </summary>
    public static class TaskEx
    {
        /// <summary>
        ///     Forgets the specified task.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="exceptionHandler">The exception handler.</param>
        public static async Task Forget(this Task task, Action<Exception> exceptionHandler = null)
        {
            try
            {
                await task.ConfigureAwait(false);
            }
            catch (Exception e)
            {
                exceptionHandler?.Invoke(e);
            }
        }
    }
}