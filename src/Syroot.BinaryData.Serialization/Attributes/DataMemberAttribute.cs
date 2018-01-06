using System;

namespace Syroot.BinaryData
{
    /// <summary>
    /// Configures that a member is explicitly read or written through binary serialization. Use this attribute if the
    /// owning instance is marked as <see cref="DataClassAttribute.Explicit"/> and no other attributes are meaningful
    /// for the member.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class DataMemberAttribute : Attribute { }
}
