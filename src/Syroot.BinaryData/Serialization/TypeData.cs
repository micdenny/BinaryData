using System;
using System.Collections.Generic;
using System.Reflection;
using Syroot.BinaryData.Core;

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

            // Get the type configuration.
            ClassConfig = Type.GetCustomAttribute<DataClassAttribute>() ?? new DataClassAttribute();
            OffsetStartConfig = Type.GetCustomAttribute<DataOffsetStartAttribute>();
            OffsetEndConfig = Type.GetCustomAttribute<DataOffsetEndAttribute>();

            // Get the member configurations, and collect a parameterless constructor on the way.
            Members = new List<MemberData>();
            foreach (MemberInfo member in Type.GetMembers(
                BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                switch (member)
                {
                    case ConstructorInfo constructorInfo:
                        if (constructorInfo.GetParameters().Length == 0)
                            Constructor = constructorInfo;
                        break;
                    case FieldInfo field:
                        AnalyzeField(field);
                        break;
                    case PropertyInfo property:
                        AnalyzeProperty(property);
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
        /// Gets the <see cref="DataClassAttribute"/> configuring how the object is read and written.
        /// </summary>
        internal DataClassAttribute ClassConfig { get; }

        internal DataOffsetStartAttribute OffsetStartConfig { get; }

        internal DataOffsetEndAttribute OffsetEndConfig { get; }

        /// <summary>
        /// Gets a parameterless <see cref="ConstructorInfo"/> to instantiate the class.
        /// </summary>
        internal ConstructorInfo Constructor { get; }

        /// <summary>
        /// Gets the list of <see cref="MemberData"/> which are read and written.
        /// </summary>
        internal IList<MemberData> Members { get; }

        // ---- METHODS (INTERNAL) -------------------------------------------------------------------------------------
        
        /// <summary>
        /// Gets the <see cref="TypeData"/> instance for the given <paramref name="type"/> and caches the information on
        /// it if necessary.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to query information about.</param>
        /// <returns>The <see cref="TypeData"/> instance holding information about the type.</returns>
        internal static TypeData GetTypeData(Type type)
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

        private void AnalyzeField(FieldInfo field)
        {
            // Get any possible attribute.
            DataArrayAttribute arrayAttrib = field.GetCustomAttribute<DataArrayAttribute>();
            DataBooleanAttribute booleanAttrib = field.GetCustomAttribute<DataBooleanAttribute>();
            DataConverterAttribute converterAttrib = field.GetCustomAttribute<DataConverterAttribute>();
            DataDateTimeAttribute dateTimeAttrib = field.GetCustomAttribute<DataDateTimeAttribute>();
            DataEndianAttribute endianAttrib = field.GetCustomAttribute<DataEndianAttribute>();
            DataEnumAttribute enumAttrib = field.GetCustomAttribute<DataEnumAttribute>();
            DataMemberAttribute memberAttrib = field.GetCustomAttribute<DataMemberAttribute>();
            DataOffsetAttribute offsetAttrib = field.GetCustomAttribute<DataOffsetAttribute>();
            DataStringAttribute stringAttrib = field.GetCustomAttribute<DataStringAttribute>();

            // Handle field if either the class is non-explicit, the field public, or if it has any attribute.
            bool exported = (!ClassConfig.Explicit && field.IsPublic)
                || arrayAttrib != null || booleanAttrib != null || converterAttrib != null || dateTimeAttrib != null
                || endianAttrib != null || enumAttrib != null || memberAttrib != null || offsetAttrib != null
                || stringAttrib != null;
            if (exported)
            {
                // Fields of enumerable type must be decorated with the DataArrayAttribute.
                if (field.FieldType.IsEnumerable() && arrayAttrib == null)
                {
                    throw new InvalidOperationException(
                        $"Enumerable field \"{field}\" must be decorated with a {nameof(DataArrayAttribute)}.");
                }
                Members.Add(new MemberData(field, field.FieldType, arrayAttrib, booleanAttrib, converterAttrib,
                    dateTimeAttrib, endianAttrib, enumAttrib, memberAttrib, offsetAttrib, stringAttrib));
            }
        }

        private void AnalyzeProperty(PropertyInfo property)
        {
            // Get any possible attribute.
            DataArrayAttribute arrayAttrib = property.GetCustomAttribute<DataArrayAttribute>();
            DataBooleanAttribute booleanAttrib = property.GetCustomAttribute<DataBooleanAttribute>();
            DataConverterAttribute converterAttrib = property.GetCustomAttribute<DataConverterAttribute>();
            DataDateTimeAttribute dateTimeAttrib = property.GetCustomAttribute<DataDateTimeAttribute>();
            DataEndianAttribute endianAttrib = property.GetCustomAttribute<DataEndianAttribute>();
            DataEnumAttribute enumAttrib = property.GetCustomAttribute<DataEnumAttribute>();
            DataMemberAttribute memberAttrib = property.GetCustomAttribute<DataMemberAttribute>();
            DataOffsetAttribute offsetAttrib = property.GetCustomAttribute<DataOffsetAttribute>();
            DataStringAttribute stringAttrib = property.GetCustomAttribute<DataStringAttribute>();

            // Handle property of either the class is non-explicit, the property public, has any attribute, and has a
            // getter and setter.
            bool exported = (!ClassConfig.Explicit
                && property.GetMethod?.IsPublic == true && property.SetMethod?.IsPublic == true)
                || arrayAttrib != null || booleanAttrib != null || converterAttrib != null || dateTimeAttrib != null
                || endianAttrib != null || enumAttrib != null || memberAttrib != null || offsetAttrib != null
                || stringAttrib != null;
            if (exported)
            {
                // Properties of enumerable type must be decorated with the DataArrayAttribute.
                if (property.PropertyType.IsEnumerable() && arrayAttrib == null)
                {
                    throw new InvalidOperationException(
                        $"Enumerable property \"{property}\" must be decorated with a {nameof(DataArrayAttribute)}.");
                }
                Members.Add(new MemberData(property, property.PropertyType, arrayAttrib, booleanAttrib, converterAttrib,
                    dateTimeAttrib, endianAttrib, enumAttrib, memberAttrib, offsetAttrib, stringAttrib));
            }
        }
    }
}
