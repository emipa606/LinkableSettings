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


    public static readonly Dictionary<string, float> VanillaFacilityRange =
        new Dictionary<string, float>();

    public static readonly Dictionary<string, int> VanillaFacilityType =
        new Dictionary<string, int>();

    public static readonly Dictionary<string, int> VanillaFacilityAmount =
        new Dictionary<string, int>();

    public static readonly Texture2D Search = ContentFinder<Texture2D>.Get("Icons/magnify");

    static Main()
    {
        var harmony = new Harmony("Mlie.LinkableSettings");
        harmony.PatchAll(Assembly.GetExecutingAssembly());
        AllFacilities = DefDatabase<ThingDef>.AllDefsListForReading
            .Where(def => def.HasComp(typeof(CompFacility))).ToList();
        if (LinkableSettingsMod.instance.Settings.FacilityType == null)
        {
            LinkableSettingsMod.instance.Settings.FacilityType = new Dictionary<string, int>();
        }

        if (LinkableSettingsMod.instance.Settings.FacilityRange == null)
        {
            LinkableSettingsMod.instance.Settings.FacilityRange = new Dictionary<string, float>();
        }

        if (LinkableSettingsMod.instance.Settings.FacilityAmount == null)
        {
            LinkableSettingsMod.instance.Settings.FacilityAmount = new Dictionary<string, int>();
        }

        SaveVanilla();
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

            if (LinkableSettingsMod.instance.Settings.FacilityType.ContainsKey(facility.defName))
            {
                facilityComp.mustBePlacedAdjacent =
                    LinkableSettingsMod.instance.Settings.FacilityType[facility.defName] == 1;
                facilityComp.mustBePlacedAdjacentCardinalToBedHead =
                    LinkableSettingsMod.instance.Settings.FacilityType[facility.defName] == 2;
                facilityComp.mustBePlacedAdjacentCardinalToAndFacingBedHead =
                    LinkableSettingsMod.instance.Settings.FacilityType[facility.defName] == 3;
            }

            if (LinkableSettingsMod.instance.Settings.FacilityRange.ContainsKey(facility.defName))
            {
                facilityComp.maxDistance = LinkableSettingsMod.instance.Settings.FacilityRange[facility.defName];
            }

            if (LinkableSettingsMod.instance.Settings.FacilityAmount.ContainsKey(facility.defName))
            {
                facilityComp.maxSimultaneous = LinkableSettingsMod.instance.Settings.FacilityAmount[facility.defName];
            }
        }
    }

    public static bool HaveAnySavedSettings()
    {
        return LinkableSettingsMod.instance.Settings.FacilityType.Any() ||
               LinkableSettingsMod.instance.Settings.FacilityRange.Any() ||
               LinkableSettingsMod.instance.Settings.FacilityAmount.Any();
    }

    public static bool HaveAnySavedSettings(string defName)
    {
        return LinkableSettingsMod.instance.Settings.FacilityType.ContainsKey(defName) ||
               LinkableSettingsMod.instance.Settings.FacilityRange.ContainsKey(defName) ||
               LinkableSettingsMod.instance.Settings.FacilityAmount.ContainsKey(defName);
    }

    public static void SaveVanilla()
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

        if (!VanillaFacilityType.ContainsKey(facility.defName))
        {
            return;
        }

        facilityComp.mustBePlacedAdjacent = VanillaFacilityType[facility.defName] == 1;
        facilityComp.mustBePlacedAdjacentCardinalToBedHead = VanillaFacilityType[facility.defName] == 2;
        facilityComp.mustBePlacedAdjacentCardinalToAndFacingBedHead = VanillaFacilityType[facility.defName] == 3;
        facilityComp.maxDistance = VanillaFacilityRange[facility.defName];
        facilityComp.maxSimultaneous = VanillaFacilityAmount[facility.defName];
        LinkableSettingsMod.instance.Settings.FacilityRange.Remove(facility.defName);
        LinkableSettingsMod.instance.Settings.FacilityType.Remove(facility.defName);
        LinkableSettingsMod.instance.Settings.FacilityAmount.Remove(facility.defName);
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

            if (!VanillaFacilityType.ContainsKey(facility.defName))
            {
                continue;
            }

            facilityComp.mustBePlacedAdjacent = VanillaFacilityType[facility.defName] == 1;
            facilityComp.mustBePlacedAdjacentCardinalToBedHead = VanillaFacilityType[facility.defName] == 2;
            facilityComp.mustBePlacedAdjacentCardinalToAndFacingBedHead = VanillaFacilityType[facility.defName] == 3;
            facilityComp.maxDistance = VanillaFacilityRange[facility.defName];
            facilityComp.maxSimultaneous = VanillaFacilityAmount[facility.defName];
        }

        LinkableSettingsMod.instance.Settings.FacilityRange = new Dictionary<string, float>();
        LinkableSettingsMod.instance.Settings.FacilityType = new Dictionary<string, int>();
        LinkableSettingsMod.instance.Settings.FacilityAmount = new Dictionary<string, int>();
    }

    public static void LogMessage(string message, bool forced = false, bool warning = false)
    {
        if (warning)
        {
            Log.Warning($"[LinkableSettings]: {message}");
            return;
        }

        if (!forced && !LinkableSettingsMod.instance.Settings.VerboseLogging)
        {
            return;
        }

        Log.Message($"[LinkableSettings]: {message}");
    }
}