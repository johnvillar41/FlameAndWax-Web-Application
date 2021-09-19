using System.Runtime.Serialization;

namespace FlameAndWax.Data.Constants
{
    public enum RegionType
    {
        [EnumMember(Value = "National Capital Region")]
        NCR,
        
        [EnumMember(Value ="Cordillera Administrative Region")]
        CAR,

        [EnumMember(Value ="Ilocos Region")]
        Region1,

        [EnumMember(Value ="Cagayan Valley")]
        Region2,

        [EnumMember(Value ="Central Luzon")]
        Region3,

        [EnumMember(Value ="Calabarzon")]
        Region4_A,

        [EnumMember(Value = "Mimaropa")]
        Southern_West_Tagalog_Region,

        [EnumMember(Value ="Bicol Region")]
        Region5,

        [EnumMember(Value ="Western Visayas")]
        Region6,

        [EnumMember(Value ="Central Visayas")]
        Region7,

        [EnumMember(Value ="Eastern Visayas")]
        Region8,

        [EnumMember(Value ="Zamboanga Peninsula")]
        Region9,

        [EnumMember(Value ="Northern Mindanao")]
        Region10,

        [EnumMember(Value ="Davao Region")]
        Region11,

        [EnumMember(Value ="Soccsksargen")]
        Region12,

        [EnumMember(Value ="Caraga")]
        Region13,

        [EnumMember(Value ="Bangsamoro")]
        BARMM

    }
}
