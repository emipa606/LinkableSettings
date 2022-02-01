using HarmonyLib;
using RimWorld;
using Verse;

namespace LinkableSettings;

[HarmonyPatch(typeof(CompAffectedByFacilities), "CanPotentiallyLinkTo_Static", typeof(ThingDef), typeof(IntVec3),
    typeof(Rot4), typeof(ThingDef), typeof(IntVec3), typeof(Rot4))]
public static class CompAffectedByFacilities_CanPotentiallyLinkTo_Static
{
    public static bool Prefix(ThingDef facilityDef, IntVec3 facilityPos, ThingDef myDef, IntVec3 myPos,
        ref bool __result)
    {
        if (!LinkableSettingsMod.instance.Settings.FacilityType.ContainsKey(facilityDef.defName) ||
            LinkableSettingsMod.instance.Settings.FacilityType[facilityDef.defName] != 4)
        {
            return true;
        }

        var currentMap = Find.CurrentMap;
        if (currentMap == null)
        {
            return true;
        }

        try
        {
            var facilityRoom = facilityPos.GetRoom(currentMap);
            if (facilityRoom.PsychologicallyOutdoors)
            {
                return true;
            }

            __result = facilityRoom == myPos.GetRoom(currentMap);
        }
        catch
        {
            // ignored. Not very nice but there are a lot of things that could go wrong here
            return true;
        }

        return false;
    }
}