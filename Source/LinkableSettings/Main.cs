using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace LinkableSettings;

[StaticConstructorOnStartup]
public static class Main
{
    public static readonly List<ThingDef> AllFacilities;

    public static readonly Dictionary<string, float> VanillaFacilityRange = new();

    public static readonly Dictionary<string, int> VanillaFacilityType = new();

    public static readonly Dictionary<string, int> VanillaFacilityAmount = new();

    public static readonly Texture2D Search = ContentFinder<Texture2D>.Get("Icons/magnify");

    static Main()
    {
        var harmony = new Harmony("Mlie.LinkableSettings");
        harmony.PatchAll(Assembly.GetExecutingAssembly());
        AllFacilities = DefDatabase<ThingDef>.AllDefsListForReading
            .Where(def => (def.HasComp<CompFacility>() || def.HasComp<CompFacilityInactiveWhenElectricityDisabled>() ||
                           def.HasComp<CompFacilityQualityBased>()) &&
                          !def.comps.Any(properties => properties is CompProperties_GravshipFacility)).ToList();
        LinkableSettingsMod.Instance.Settings.FacilityType ??= new Dictionary<string, int>();

        LinkableSettingsMod.Instance.Settings.FacilityRange ??= new Dictionary<string, float>();

        LinkableSettingsMod.Instance.Settings.FacilityAmount ??= new Dictionary<string, int>();

        saveVanilla();
        UpdateFacilities();
    }

    public static void UpdateFacilities()
    {
        foreach (var facility in AllFacilities)
        {
            var facilityComp = facility.GetCompProperties<CompProperties_Facility>();
            if (facilityComp == null)
            {
                continue;
            }

            if (LinkableSettingsMod.Instance.Settings.FacilityType.ContainsKey(facility.defName))
            {
                facilityComp.mustBePlacedAdjacent =
                    LinkableSettingsMod.Instance.Settings.FacilityType[facility.defName] == 1;
                facilityComp.mustBePlacedAdjacentCardinalToBedHead =
                    LinkableSettingsMod.Instance.Settings.FacilityType[facility.defName] == 2;
                facilityComp.mustBePlacedAdjacentCardinalToAndFacingBedHead =
                    LinkableSettingsMod.Instance.Settings.FacilityType[facility.defName] == 3;
            }
            else
            {
                facilityComp.mustBePlacedAdjacent = VanillaFacilityType[facility.defName] == 1;
                facilityComp.mustBePlacedAdjacentCardinalToBedHead = VanillaFacilityType[facility.defName] == 2;
                facilityComp.mustBePlacedAdjacentCardinalToAndFacingBedHead =
                    VanillaFacilityType[facility.defName] == 3;
            }

            facilityComp.maxDistance =
                LinkableSettingsMod.Instance.Settings.FacilityRange.TryGetValue(facility.defName, out var distance)
                    ? distance
                    : VanillaFacilityRange[facility.defName];

            facilityComp.maxSimultaneous =
                LinkableSettingsMod.Instance.Settings.FacilityAmount.TryGetValue(facility.defName, out var amount)
                    ? amount > 25 ? int.MaxValue : amount
                    : VanillaFacilityAmount[facility.defName];
        }
    }

    public static bool HaveAnySavedSettings()
    {
        return LinkableSettingsMod.Instance.Settings.FacilityType.Any() ||
               LinkableSettingsMod.Instance.Settings.FacilityRange.Any() ||
               LinkableSettingsMod.Instance.Settings.FacilityAmount.Any();
    }

    public static bool HaveAnySavedSettings(string defName)
    {
        return LinkableSettingsMod.Instance.Settings.FacilityType.ContainsKey(defName) ||
               LinkableSettingsMod.Instance.Settings.FacilityRange.ContainsKey(defName) ||
               LinkableSettingsMod.Instance.Settings.FacilityAmount.ContainsKey(defName);
    }

    private static void saveVanilla()
    {
        foreach (var facility in AllFacilities)
        {
            var facilityComp = facility.GetCompProperties<CompProperties_Facility>();
            if (facilityComp == null)
            {
                continue;
            }

            VanillaFacilityType[facility.defName] = 0;

            if (facilityComp.mustBePlacedAdjacent)
            {
                VanillaFacilityType[facility.defName] = 1;
            }

            if (facilityComp.mustBePlacedAdjacentCardinalToBedHead)
            {
                VanillaFacilityType[facility.defName] = 2;
            }

            if (facilityComp.mustBePlacedAdjacentCardinalToAndFacingBedHead)
            {
                VanillaFacilityType[facility.defName] = 3;
            }

            VanillaFacilityRange[facility.defName] = facilityComp.maxDistance;
            VanillaFacilityAmount[facility.defName] = facilityComp.maxSimultaneous;
        }
    }

    public static void ResetToVanilla(string defName)
    {
        var facility = DefDatabase<ThingDef>.GetNamedSilentFail(defName);

        var facilityComp = facility?.GetCompProperties<CompProperties_Facility>();
        if (facilityComp == null)
        {
            return;
        }

        if (!VanillaFacilityType.TryGetValue(facility.defName, out var value))
        {
            return;
        }

        facilityComp.mustBePlacedAdjacent = value == 1;
        facilityComp.mustBePlacedAdjacentCardinalToBedHead = VanillaFacilityType[facility.defName] == 2;
        facilityComp.mustBePlacedAdjacentCardinalToAndFacingBedHead = VanillaFacilityType[facility.defName] == 3;
        facilityComp.maxDistance = VanillaFacilityRange[facility.defName];
        facilityComp.maxSimultaneous = VanillaFacilityAmount[facility.defName];
        LinkableSettingsMod.Instance.Settings.FacilityRange.Remove(facility.defName);
        LinkableSettingsMod.Instance.Settings.FacilityType.Remove(facility.defName);
        LinkableSettingsMod.Instance.Settings.FacilityAmount.Remove(facility.defName);
    }

    public static void ResetToVanilla()
    {
        foreach (var facility in AllFacilities)
        {
            var facilityComp = facility.GetCompProperties<CompProperties_Facility>();
            if (facilityComp == null)
            {
                continue;
            }

            if (!VanillaFacilityType.TryGetValue(facility.defName, out var value))
            {
                continue;
            }

            facilityComp.mustBePlacedAdjacent = value == 1;
            facilityComp.mustBePlacedAdjacentCardinalToBedHead = VanillaFacilityType[facility.defName] == 2;
            facilityComp.mustBePlacedAdjacentCardinalToAndFacingBedHead = VanillaFacilityType[facility.defName] == 3;
            facilityComp.maxDistance = VanillaFacilityRange[facility.defName];
            facilityComp.maxSimultaneous = VanillaFacilityAmount[facility.defName];
        }

        LinkableSettingsMod.Instance.Settings.FacilityRange = new Dictionary<string, float>();
        LinkableSettingsMod.Instance.Settings.FacilityType = new Dictionary<string, int>();
        LinkableSettingsMod.Instance.Settings.FacilityAmount = new Dictionary<string, int>();
    }
}