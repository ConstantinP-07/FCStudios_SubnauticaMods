﻿using FCS_AlterraHub.Objects;
using UnityEngine;

namespace FCS_AlterraHub.Extensions
{
    /// <summary>
    /// Extensions that handle color conversion
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        /// Converts a <see cref="Color"/> to a <see cref="ColorVec4"/>
        /// </summary>
        /// <param name="color">The color to convert</param>
        /// <returns></returns>
        public static ColorVec4 ColorToVector4(this Color color)
        {
            return new ColorVec4(color.r, color.g, color.b, color.a);
        }

        /// <summary>
        /// Converts a <see cref="ColorVec4"/> to a <see cref="Color"/>
        /// </summary>
        /// <param name="vec4"></param>
        /// <returns></returns>
        public static Color Vector4ToColor(this ColorVec4 vec4)
        {
            return new Color(vec4.R, vec4.G, vec4.B, vec4.A);
        }

        /// <summary>
        /// Converts a <see cref="Vec3"/> to a <see cref="Vector3"/>
        /// </summary>
        /// <param name="vec3"></param>
        /// <returns></returns>
        public static Color Vec3ToColor(this Vec3 vec3)
        {
            return new Color(vec3.X, vec3.Y, vec3.Z);
        }

    }
}