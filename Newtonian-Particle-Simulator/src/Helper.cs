﻿using System;
using System.IO;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Newtonian_Particle_Simulator
{
    static class Helper
    {
        public static readonly double APIVersion = Convert.ToDouble(GL.GetString(StringName.Version).Substring(0, 3));
        public static readonly double GLSLVersion = Convert.ToDouble(GL.GetString(StringName.ShadingLanguageVersion));

        public static string GetPathContent(this string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"{path} does not exist");

            return File.ReadAllText(path);
        }

        public static Vector3 RandomUnitVector(Random rng)
        {
            float z = (float)rng.NextDouble() * 2.0f - 1.0f;
            float a = (float)rng.NextDouble() * 2.0f * MathF.PI;
            float r = MathF.Sqrt(1.0f - z * z);
            float x = r * MathF.Cos(a);
            float y = r * MathF.Sin(a);

            return new Vector3(x, y, z);
        }

        private static HashSet<string> GetExtensions()
        {
            HashSet<string> extensions = new HashSet<string>(GL.GetInteger(GetPName.NumExtensions));
            for (int i = 0; i < GL.GetInteger(GetPName.NumExtensions); i++)
                extensions.Add(GL.GetString(StringNameIndexed.Extensions, i));
            
            return extensions;
        }

        private static readonly HashSet<string> glExtensions = new HashSet<string>(GetExtensions());

        /// <summary>
        /// </summary>
        /// <param name="extension">The extension to check against. Examples: GL_ARB_bindless_texture or WGL_EXT_swap_control</param>
        /// <returns>True if the extension is available</returns>
        public static bool IsExtensionsAvailable(string extension)
        {
            return glExtensions.Contains(extension);
        }

        /// <summary>
        /// </summary>
        /// <param name="extension">The extension to check against. Examples: GL_ARB_direct_state_access or GL_ARB_compute_shader</param>
        /// <param name="first">The major API version the extension became part of the core profile</param>
        /// <param name="last">The minor API version the extension became part of the core profile</param>
        /// <returns>True if this OpenGL version is in the specified range or the extension is otherwise available</returns>
        public static bool IsCoreExtensionAvailable(string extension, double first, double last)
        {
            return (APIVersion >= first && APIVersion <= last) || IsExtensionsAvailable(extension);
        }
    }
}