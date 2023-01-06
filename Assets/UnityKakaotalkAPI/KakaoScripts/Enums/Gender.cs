using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Runtime.Serialization;

namespace Kakaotalk {
    public enum Gender {
        [EnumMember(Value = "female")]
        FEMALE,
        [EnumMember(Value = "male")]
        MALE,
        UNKNOWN
    }
}