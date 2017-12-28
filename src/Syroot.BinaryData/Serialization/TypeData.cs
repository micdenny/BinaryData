using System;
using System.Collections.Generic;
using System.Reflection;

namespace Syroot.BinaryData.Serialization
{
    /// <summary>
    /// Represents reflected type configuration required for reading and writing it as binary data.
    /// </summary>
    internal class TypeData
    {
        // ---- FIELDS -------------------------------------------------------------------------------------------------

        private static readonly Dictionary<Type, TypeData> _cache = new Dictionary<Type, TypeData>();
        
        // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

        private TypeData(Type type)
        {
            Type = type;

            // Get any possible attribute.
            DataClassAttribute classAttrib = Type.GetCustomAttribute<DataClassAttribute>();
            if (classAttrib != null)
            {
                Inherit = classAttrib.Inherit;
                Explicit = classAttrib.Explicit;
            }
            DataOffsetStartAttribute offsetStartAttrib = Type.GetCustomAttribute<DataOffsetStartAttribute>();
            if (offsetStartAttrib != null)
            {
                StartOrigin = offsetStartAttrib.Origin;
                StartDelta = offsetStartAttrib.Delta;
            }
            DataOffsetEndAttribute offsetEndAttrib = Type.GetCustomAttribute<DataOffsetEndAttribute>();
            if (offsetEndAttrib != null)
            {
                EndOrigin = offsetEndAttrib.Origin;
                EndDelta = offsetEndAttrib.Delta;
            }

            // Get the member configurations, and collect a parameterless constructor on the way.
            OrderedMembers = new SortedDictionary<int, MemberData>();
            UnorderedMembers = new SortedList<string, MemberData>(StringComparer.Ordinal);
            foreach (MemberInfo member in Type.GetMembers(
                BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                switch (member)
                {
                    case ConstructorInfo constructorInfo:
                        if (constructorInfo.GetParameters().Length == 0)
                            Constructor = constructorInfo;
                        break;
                    case FieldInfo fieldInfo:
                        AnalyzeMember(new MemberData(fieldInfo), fieldInfo.IsPublic);
                        break;
                    case PropertyInfo propertyInfo:
                        AnalyzeMember(new MemberData(propertyInfo),
                            propertyInfo.GetMethod?.IsPublic == true && propertyInfo.SetMethod?.IsPublic == true);
                        break;
                }
            }
        }

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        /// <summary>
        /// Gets the <see cref="Type"/> to which informations are stored.
        /// </summary>
        internal Type Type { get; }

        /// <summary>
        /// Gets a parameterless <see cref="ConstructorInfo"/> to instantiate the class.
        /// </summary>
        internal ConstructorInfo Constructor { get; }

        /// <summary>
        /// Gets the dictionary of <see cref="MemberData"/> for members decorated with the
        /// <see cref="DataOrderAttribute"/>.
        /// </summary>
        internal SortedDictionary<int, MemberData> OrderedMembers { get; }

        /// <summary>
        /// Gets the list of <see cref="MemberData"/> for members missing the <see cref="DataOrderAttribute"/>.
        /// </summary>
        internal SortedList<string, MemberData> UnorderedMembers { get; }

        // ---- DataClassAttribute ----

        /// <summary>
        /// Gets or sets a value indicating whether inherited members are read and written first.
        /// </summary>
        internal bool Inherit { get; }

        /// <summary>
        /// Gets or sets a value indicating whether public members are not automatically read and written.
        /// </summary>
        internal bool Explicit { get; }

        // ---- DataOffsetStartAttribute ----

        /// <summary>
        /// Gets or sets the anchor from which to manipulate the stream position by the delta before reading the
        /// instance.
        /// </summary>
        internal Origin StartOrigin { get; }

        /// <summary>
        /// Gets or sets the number of bytes to manipulate the stream position with before reading the instance.
        /// </summary>
        internal long StartDelta { get; }

        // ---- DataOffsetEndAttribute ----

        /// <summary>
        /// Gets or sets the anchor from which to manipulate the stream position by the delta after reading the
        /// instance.
        /// </summary>
        internal Origin EndOrigin { get; }

        /// <summary>
        /// Gets or sets the number of bytes to manipulate the stream position with after reading the instance.
        /// </summary>
        internal long EndDelta { get; }

        // ---- METHODS (INTERNAL) -------------------------------------------------------------------------------------
        
        /// <summary>
        /// Gets the <see cref="TypeData"/> instance for the given <paramref name="type"/> and caches the information on
        /// it if necessary.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to query information about.</param>
        /// <returns>The <see cref="TypeData"/> instance holding information about the type.</returns>
        internal static TypeData Get(Type type)
        {
            if (!_cache.TryGetValue(type, out TypeData typeData))
            {
                typeData = new TypeData(type);
                _cache.Add(type, typeData);
            }
            return typeData;
        }

        /// <summary>
        /// Invokes the parameterless constructor on the object.
        /// </summary>
        /// <returns>A new instance of the object.</returns>
        internal object Instantiate()
        {
            // Invoke the automatic default constructor for structs.
            if (Type.IsValueType)
            {
                return Activator.CreateInstance(Type);
            }

            // Invoke an explicit parameterless constructor for classes.
            if (Constructor == null)
            {
                throw new MissingMethodException($"No parameterless constructor found for {Type}.");
            }
            return Constructor.Invoke(null);
        }

        // ---- METHODS (PRIVATE) --------------------------------------------------------------------------------------
        
        private void AnalyzeMember(MemberData memberData, bool isPublic)
        {
            if (memberData.IsExported || (!ClassAttrib.Explicit && isPublic))
            {
                if (memberData.Index == Int32.MinValue)
                {
                    UnorderedMembers.Add(memberData.MemberInfo.Name, memberData);
                }
                else
                {
                    OrderedMembers.Add(memberData.Index, memberData);
                }
            }
        }
    }
}
