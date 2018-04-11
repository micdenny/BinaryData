using System;
using System.Collections;
using System.Collections.Generic;

namespace Syroot.BinaryData
{
    /// <summary>
    /// Represents a collection of extension methods for the <see cref="Type"/> class.
    /// </summary>
    internal static class TypeExtensions
    {
        // ---- METHODS (INTERNAL) -------------------------------------------------------------------------------------

        /// <summary>
        /// Returns a value indicating whether the given <paramref name="self"/> is enumerable. Returns <c>false</c> for
        /// non-enumerable objects and strings.
        /// </summary>
        /// <param name="self">The extended <see cref="Type"/> instance.</param>
        /// <returns><c>true</c> if the type is enumerable and not a string; otherwise <c>false</c>.</returns>
        internal static bool IsEnumerable(this Type self)
        {
            return self != typeof(string) && typeof(IEnumerable).IsAssignableFrom(self);
        }

        /// <summary>
        /// Gets the element type of <see cref="IEnumerable"/> instances. Returns <c>null</c> for non-enumerable
        /// objects.
        /// </summary>
        /// <param name="self">The extended <see cref="Type"/> instance.</param>
        /// <returns>The type of the elements, or <c>null</c>.</returns>
        internal static Type GetEnumerableElementType(this Type self)
        {
            // Check for array instances.
            if (self.IsArray)
            {
                Type elementType;
                if (self.GetArrayRank() > 1 || (elementType = self.GetElementType()).IsArray)
                {
                    throw new NotImplementedException(
                        $"Type {self} is a multi-dimensional array and is not supported at the moment.");
                }
                return elementType;
            }

            // Check for IEnumerable instances. Only the first implementation of IEnumerable<> is returned.
            if (typeof(IEnumerable).IsAssignableFrom(self))
            {
                foreach (Type interfaceType in self.GetInterfaces())
                {
                    if (interfaceType.IsGenericType
                        && interfaceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    {
                        return interfaceType.GetGenericArguments()[0];
                    }
                }
            }

            return null;
        }

        internal static bool TryGetEnumerableElementType(this Type self, out Type elementType)
        {
            // Check for array instances.
            if (self.IsArray)
            {
                Type elemType;
                if (self.GetArrayRank() > 1 || (elemType = self.GetElementType()).IsArray)
                {
                    throw new NotImplementedException(
                        $"Type {self} is a multidimensional array and is not supported at the moment.");
                }
                elementType = elemType;
                return true;
            }

            // Check for IEnumerable instances. Only the first implementation of IEnumerable<> is returned.
            if (typeof(IEnumerable).IsAssignableFrom(self))
            {
                foreach (Type interfaceType in self.GetInterfaces())
                {
                    if (interfaceType.IsGenericType
                        && interfaceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    {
                        elementType = interfaceType.GetGenericArguments()[0];
                        return true;
                    }
                }
            }

            elementType = null;
            return false;
        }
    }
}
