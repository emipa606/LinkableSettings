using System.Collections.Generic;
using Verse;

namespace LinkableSettings;

/// <summary>
///     Definition of the settings for the mod
/// </summary>
internal class LinkableSettingsModSettings : ModSettings
{
    public Dictionary<string, int> FacilityAmount = new();

    private List<string> facilityAmountKeys;

    private List<int> facilityAmountValues;

    public Dictionary<string, float> FacilityRange = new();

    private List<string> facilityRangeKeys;

    private List<float> facilityRangeValues;

    public Dictionary<string, int> FacilityType = new();

    private List<string> facilityTypeKeys;

    private List<int> facilityTypeValues;
    public bool VerboseLogging;

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