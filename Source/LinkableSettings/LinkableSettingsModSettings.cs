using System.Collections.Generic;
using Verse;

namespace LinkableSettings;

/// <summary>
///     Definition of the settings for the mod
/// </summary>
internal class LinkableSettingsModSettings : ModSettings
{
    public Dictionary<string, int> FacilityAmount = new();

    public Dictionary<string, float> FacilityRange = new();

    public Dictionary<string, int> FacilityType = new();
    public bool VerboseLogging;

    private List<string> facilityAmountKeys;

    private List<int> facilityAmountValues;

    private List<string> facilityRangeKeys;

    private List<float> facilityRangeValues;

    private List<string> facilityTypeKeys;

    private List<int> facilityTypeValues;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref VerboseLogging, "VerboseLogging");
        Scribe_Collections.Look(ref FacilityType, "FacilityType", LookMode.Value,
            LookMode.Value,
            ref facilityTypeKeys, ref facilityTypeValues);
        Scribe_Collections.Look(ref FacilityAmount, "FacilityAmount", LookMode.Value,
            LookMode.Value,
            ref facilityAmountKeys, ref facilityAmountValues);
        Scribe_Collections.Look(ref FacilityRange, "FacilityRange", LookMode.Value,
            LookMode.Value,
            ref facilityRangeKeys, ref facilityRangeValues);
    }
}