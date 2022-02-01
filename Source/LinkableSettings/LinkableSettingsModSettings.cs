using System.Collections.Generic;
using Verse;

namespace LinkableSettings;

/// <summary>
///     Definition of the settings for the mod
/// </summary>
internal class LinkableSettingsModSettings : ModSettings
{
    public Dictionary<string, int> FacilityAmount =
        new Dictionary<string, int>();

    private List<string> FacilityAmountKeys;

    private List<int> FacilityAmountValues;

    public Dictionary<string, float> FacilityRange =
        new Dictionary<string, float>();

    private List<string> FacilityRangeKeys;

    private List<float> FacilityRangeValues;

    public Dictionary<string, int> FacilityType =
        new Dictionary<string, int>();

    private List<string> FacilityTypeKeys;

    private List<int> FacilityTypeValues;
    public bool VerboseLogging;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref VerboseLogging, "VerboseLogging");
        Scribe_Collections.Look(ref FacilityType, "FacilityType", LookMode.Value,
            LookMode.Value,
            ref FacilityTypeKeys, ref FacilityTypeValues);
        Scribe_Collections.Look(ref FacilityAmount, "FacilityAmount", LookMode.Value,
            LookMode.Value,
            ref FacilityAmountKeys, ref FacilityAmountValues);
        Scribe_Collections.Look(ref FacilityRange, "FacilityRange", LookMode.Value,
            LookMode.Value,
            ref FacilityRangeKeys, ref FacilityRangeValues);
    }
}