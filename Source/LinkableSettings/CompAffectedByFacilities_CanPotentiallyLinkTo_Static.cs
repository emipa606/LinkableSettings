using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace LinkableSettings;

[HarmonyPatch(typeof(CompAffectedByFacilities), "CanPotentiallyLinkTo_Static", typeof(ThingDef), typeof(IntVec3),
    typeof(Rot4), typeof(ThingDef), typeof(IntVec3), typeof(Rot4), typeof(Map))]
public static class CompAffectedByFacilities_CanPotentiallyLinkTo_Static
{
    public static bool Prefix(ThingDef facilityDef, IntVec3 facilityPos, Rot4 facilityRot, ThingDef myDef,
        IntVec3 myPos, Rot4 myRot, ref bool __result)
    {
        if (!LinkableSettingsMod.Instance.Settings.FacilityType.TryGetValue(facilityDef.defName, out var linkType) ||
            linkType < 4)
        {
            return true;
        }

        var currentMap = Find.CurrentMap;
        if (currentMap == null)
        {
            return true;
        }

        var compProperties = facilityDef.GetCompProperties<CompProperties_Facility>();
        var myCenter = GenThing.TrueCenter(myPos, myRot, myDef.size, myDef.Altitude);
        var facilityCenter = GenThing.TrueCenter(facilityPos, facilityRot, facilityDef.size, facilityDef.Altitude);

        __result = Vector3.Distance(myCenter, facilityCenter) <= compProperties.maxDistance;

        if (linkType != 4 || !__result)
        {
            return false;
        }

        try
        {
            __result = currentMap.regionAndRoomUpdater.Enabled &&
                       facilityPos.GetRoom(currentMap) == myPos.GetRoom(currentMap);
        }
        catch
        {
            // ignored. Not very nice but there are a lot of things that could go wrong here
        }

        return false;
    }
}