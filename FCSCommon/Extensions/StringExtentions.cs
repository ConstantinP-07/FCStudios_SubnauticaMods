﻿using SMLHelper.V2.Handlers;
using System;

namespace FCSCommon.Extensions
{
    internal static class StringExtentions
    {
        /// <summary>
        /// Removes (Instance) from strings)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal static string RemoveInstance(this string name)
        {
            return name.Replace("(Instance)", string.Empty).Trim();
        }

        /// <summary>
        /// Removes (Clone) from strings)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal static string RemoveClone(this string name)
        {
            return name.Replace("(Clone)", string.Empty).Trim();
        }
    }
}
