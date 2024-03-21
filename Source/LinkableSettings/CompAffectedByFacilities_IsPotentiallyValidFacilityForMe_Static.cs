using HarmonyLib;
using RimWorld;
using Verse;

namespace LinkableSettings;

[HarmonyPatch(typeof(CompAffectedByFacilities), "IsPotentiallyValidFacilityForMe_Static", typeof(ThingDef),
    typeof(IntVec3),
    typeof(Rot4), typeof(ThingDef), typeof(IntVec3), typeof(Rot4), typeof(Map))]
public static class CompAffectedByFacilities_IsPotentiallyValidFacilityForMe_Static
{
    public static bool Prefix(ThingDef facilityDef, IntVec3 facilityPos, IntVec3 myPos, Map map, ref bool __result)
    {
        if (!LinkableSettingsMod.instance.Settings.FacilityType.TryGetValue(facilityDef.defName, out var linkType) ||
            linkType < 4)
        {
            return true;
        }

        if (linkType == 4)
        {
            try
            {
                __result = facilityPos.GetRoom(map) == myPos.GetRoom(map);
            }
            catch
            {
                // ignored. Not very nice but there are a lot of things that could go wrong here
            }

            return false;
        }

        __result = true;

        return false;
    }
}