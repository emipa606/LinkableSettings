using System;
using Mlie;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace LinkableSettings;

[StaticConstructorOnStartup]
internal class LinkableSettingsMod : Mod
{
    /// <summary>
    ///     The instance of the settings to be read by the mod
    /// </summary>
    public static LinkableSettingsMod instance;

    private static readonly Vector2 buttonSize = new Vector2(120f, 25f);

    private static readonly Vector2 iconSize = new Vector2(24f, 24f);
    private static readonly Vector2 imageSize = new Vector2(128f, 128f);

    private static readonly int buttonSpacer = 200;

    private static readonly float columnSpacer = 0.1f;

    private static float leftSideWidth;

    private static Listing_Standard listing_Standard;

    private static Vector2 tabsScrollPosition;

    private static string currentVersion;


    /// <summary>
    ///     The private settings
    /// </summary>
    private LinkableSettingsModSettings settings;


    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="content"></param>
    public LinkableSettingsMod(ModContentPack content)
        : base(content)
    {
        instance = this;
        currentVersion =
            VersionFromManifest.GetVersionFromModMetaData(
                ModLister.GetActiveModWithIdentifier("Mlie.LinkableSettings"));
    }

    private static string SelectedDef { get; set; } = "Settings";

    /// <summary>
    ///     The instance-settings for the mod
    /// </summary>
    internal LinkableSettingsModSettings Settings
    {
        get
        {
            if (settings == null)
            {
                settings = GetSettings<LinkableSettingsModSettings>();
            }

            return settings;
        }

        set => settings = value;
    }


    /// <summary>
    ///     The settings-window
    /// </summary>
    /// <param name="rect"></param>
    public override void DoSettingsWindowContents(Rect rect)
    {
        base.DoSettingsWindowContents(rect);

        var rect2 = rect.ContractedBy(1);
        leftSideWidth = rect2.ContractedBy(10).width / 5 * 2;

        listing_Standard = new Listing_Standard();

        DrawOptions(rect2);
        DrawTabsList(rect2);
        Settings.Write();
    }

    /// <summary>
    ///     The title for the mod-settings
    /// </summary>
    /// <returns></returns>
    public override string SettingsCategory()
    {
        return "Linkable Settings";
    }

    private static void DrawButton(Action action, string text, Vector2 pos)
    {
        var rect = new Rect(pos.x, pos.y, buttonSize.x, buttonSize.y);
        if (!Widgets.ButtonText(rect, text, true, false, Color.white))
        {
            return;
        }

        SoundDefOf.Designate_DragStandard_Changed.PlayOneShotOnCamera();
        action();
    }

    public override void WriteSettings()
    {
        base.WriteSettings();
        Main.UpdateFacilities();
    }

    private void DrawIcon(ThingDef thingDef, Rect rect)
    {
        if (thingDef?.graphicData?.Graphic?.MatSingle?.mainTexture == null)
        {
            return;
        }

        var texture2D = thingDef.graphicData.Graphic.MatSingle.mainTexture;
        if (thingDef.graphicData.graphicClass == typeof(Graphic_Random))
        {
            texture2D = ((Graphic_Random)thingDef.graphicData.Graphic).FirstSubgraphic().MatSingle.mainTexture;
        }

        if (thingDef.graphicData.graphicClass == typeof(Graphic_Multi))
        {
            texture2D = ((Graphic_Multi)thingDef.graphicData.Graphic).MatNorth.mainTexture;
        }

        if (texture2D.width != texture2D.height)
        {
            var ratio = (float)texture2D.width / texture2D.height;

            if (ratio < 1)
            {
                rect.x += (rect.width - (rect.width * ratio)) / 2;
                rect.width *= ratio;
            }
            else
            {
                rect.y += (rect.height - (rect.height / ratio)) / 2;
                rect.height /= ratio;
            }
        }

        GUI.DrawTexture(rect, texture2D);
    }

    private void DrawOptions(Rect rect)
    {
        var optionsOuterContainer = rect.ContractedBy(10);
        optionsOuterContainer.x += leftSideWidth + columnSpacer;
        optionsOuterContainer.width -= leftSideWidth + columnSpacer;
        Widgets.DrawBoxSolid(optionsOuterContainer, Color.grey);
        var optionsInnerContainer = optionsOuterContainer.ContractedBy(1);
        Widgets.DrawBoxSolid(optionsInnerContainer, new ColorInt(42, 43, 44).ToColor);
        var frameRect = optionsInnerContainer.ContractedBy(10);
        frameRect.x = leftSideWidth + columnSpacer + 20;
        frameRect.y += 15;
        frameRect.height -= 15;
        var contentRect = frameRect;
        contentRect.x = 0;
        contentRect.y = 0;

        switch (SelectedDef)
        {
            case null:
                return;
            case "Settings":
            {
                listing_Standard.Begin(frameRect);
                Text.Font = GameFont.Medium;
                listing_Standard.Label("LiSe.settings".Translate());
                Text.Font = GameFont.Small;
                listing_Standard.Gap();

                if (Main.HaveAnySavedSettings())
                {
                    var labelPoint = listing_Standard.Label("LiSe.resetall.label".Translate(), -1F,
                        "LiSe.resetall.tooltip".Translate());
                    DrawButton(() =>
                        {
                            Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(
                                "LiSe.resetall.confirm".Translate(),
                                Main.ResetToVanilla));
                        }, "LiSe.resetall.button".Translate(),
                        new Vector2(labelPoint.position.x + buttonSpacer, labelPoint.position.y));
                }
                else
                {
                    listing_Standard.Gap(buttonSize.y);
                }

                listing_Standard.CheckboxLabeled("LiSe.logging.label".Translate(), ref Settings.VerboseLogging,
                    "LiSe.logging.tooltip".Translate());
                if (currentVersion != null)
                {
                    listing_Standard.Gap();
                    GUI.contentColor = Color.gray;
                    listing_Standard.Label("LiSe.version.label".Translate(currentVersion));
                    GUI.contentColor = Color.white;
                }

                listing_Standard.End();
                break;
            }

            default:
            {
                var currentDef = DefDatabase<ThingDef>.GetNamedSilentFail(SelectedDef);
                listing_Standard.Begin(frameRect);
                if (currentDef == null)
                {
                    listing_Standard.Label("LiSe.error.thing".Translate(SelectedDef));
                    listing_Standard.End();
                    break;
                }

                Text.Font = GameFont.Medium;
                var labelPoint = listing_Standard.Label(currentDef.label.CapitalizeFirst(), -1F,
                    currentDef.defName);
                Text.Font = GameFont.Small;
                var modName = currentDef.modContentPack?.Name;
                var modId = currentDef.modContentPack?.PackageId;
                if (currentDef.modContentPack != null)
                {
                    listing_Standard.Label($"{modName}", -1F, modId);
                }
                else
                {
                    listing_Standard.Gap();
                }

                var description = currentDef.description;
                if (!string.IsNullOrEmpty(description))
                {
                    if (description.Length > 400)
                    {
                        description = description.Substring(0, 400) + "...";
                    }

                    Widgets.Label(new Rect(labelPoint.x, labelPoint.y + 50, 320, 150), description);
                }

                listing_Standard.Gap(150);

                var iconRect = new Rect(labelPoint.x + 325, labelPoint.y + 5, imageSize.x, imageSize.y);

                DrawIcon(currentDef, iconRect);

                listing_Standard.GapLine();
                var linkType = instance.Settings.FacilityType.ContainsKey(currentDef.defName)
                    ? instance.Settings.FacilityType[currentDef.defName]
                    : Main.VanillaFacilityType[currentDef.defName];

                Text.Font = GameFont.Medium;
                labelPoint = listing_Standard.Label("LiSe.linktype.label".Translate(), -1f,
                    "LiSe.linktype.description".Translate());
                Text.Font = GameFont.Small;

                if (Main.HaveAnySavedSettings(currentDef.defName))
                {
                    DrawButton(() => { Main.ResetToVanilla(currentDef.defName); }, "LiSe.reset.button".Translate(),
                        new Vector2(labelPoint.position.x + frameRect.width - buttonSize.x, labelPoint.position.y));
                }

                if (listing_Standard.RadioButton("LiSe.range.label".Translate(), linkType == 0, 0f,
                        "LiSe.range.description".Translate()))
                {
                    instance.Settings.FacilityType[currentDef.defName] = 0;
                }

                if (listing_Standard.RadioButton("LiSe.room.label".Translate(), linkType == 4, 0f,
                        "LiSe.room.description".Translate()))
                {
                    instance.Settings.FacilityType[currentDef.defName] = 4;
                }

                if (listing_Standard.RadioButton("LiSe.mustBePlacedAdjacent.label".Translate(), linkType == 1, 0f,
                        "LiSe.mustBePlacedAdjacent.description".Translate()))
                {
                    instance.Settings.FacilityType[currentDef.defName] = 1;
                }

                if (listing_Standard.RadioButton("LiSe.mustBePlacedAdjacentCardinalToBedHead.label".Translate(),
                        linkType == 2, 0f,
                        "LiSe.mustBePlacedAdjacentCardinalToBedHead.description".Translate()))
                {
                    instance.Settings.FacilityType[currentDef.defName] = 2;
                }

                if (listing_Standard.RadioButton(
                        "LiSe.mustBePlacedAdjacentCardinalToAndFacingBedHead.label".Translate(), linkType == 3, 0f,
                        "LiSe.mustBePlacedAdjacentCardinalToAndFacingBedHead.description".Translate()))
                {
                    instance.Settings.FacilityType[currentDef.defName] = 3;
                }

                if (instance.Settings.FacilityType.ContainsKey(currentDef.defName) &&
                    instance.Settings.FacilityType[currentDef.defName] == Main.VanillaFacilityType[currentDef.defName])
                {
                    instance.Settings.FacilityType.Remove(currentDef.defName);
                }

                listing_Standard.Gap();
                if (linkType is 0 or 4)
                {
                    var linkRange = instance.Settings.FacilityRange.ContainsKey(currentDef.defName)
                        ? instance.Settings.FacilityRange[currentDef.defName]
                        : Main.VanillaFacilityRange[currentDef.defName];

                    var currentRange = linkRange;
                    listing_Standard.Label("LiSe.linkrange.label".Translate(linkRange), -1f,
                        "LiSe.linkrange.description".Translate());

                    linkRange = (float)Math.Round(listing_Standard.Slider(linkRange, 1f, 100f));

                    if (linkRange != currentRange)
                    {
                        instance.Settings.FacilityRange[currentDef.defName] = linkRange;
                    }

                    if (instance.Settings.FacilityRange.ContainsKey(currentDef.defName) &&
                        instance.Settings.FacilityRange[currentDef.defName] ==
                        Main.VanillaFacilityRange[currentDef.defName])
                    {
                        instance.Settings.FacilityRange.Remove(currentDef.defName);
                    }

                    listing_Standard.Gap();
                }

                var linkAmount = instance.Settings.FacilityAmount.ContainsKey(currentDef.defName)
                    ? instance.Settings.FacilityAmount[currentDef.defName]
                    : Main.VanillaFacilityAmount[currentDef.defName];
                var currentAmount = linkAmount;

                listing_Standard.Label("LiSe.linkamount.label".Translate(linkAmount), -1f,
                    "LiSe.linkamount.description".Translate());

                linkAmount = (int)Math.Round(listing_Standard.Slider(linkAmount, 1f, 25f));

                if (linkAmount != currentAmount)
                {
                    instance.Settings.FacilityAmount[currentDef.defName] = linkAmount;
                }

                if (instance.Settings.FacilityAmount.ContainsKey(currentDef.defName) &&
                    instance.Settings.FacilityAmount[currentDef.defName] ==
                    Main.VanillaFacilityAmount[currentDef.defName])
                {
                    instance.Settings.FacilityAmount.Remove(currentDef.defName);
                }

                listing_Standard.End();
                break;
            }
        }
    }

    private void DrawTabsList(Rect rect)
    {
        var scrollContainer = rect.ContractedBy(10);
        scrollContainer.width = leftSideWidth;
        Widgets.DrawBoxSolid(scrollContainer, Color.grey);
        var innerContainer = scrollContainer.ContractedBy(1);
        Widgets.DrawBoxSolid(innerContainer, new ColorInt(42, 43, 44).ToColor);
        var tabFrameRect = innerContainer.ContractedBy(5);
        tabFrameRect.y += 15;
        tabFrameRect.height -= 15;
        var tabContentRect = tabFrameRect;
        tabContentRect.x = 0;
        tabContentRect.y = 0;
        tabContentRect.width -= 20;
        var allFacilities = Main.AllFacilities;
        var listAddition = 24;

        tabContentRect.height = (allFacilities.Count * 25f) + listAddition;
        Widgets.BeginScrollView(tabFrameRect, ref tabsScrollPosition, tabContentRect);
        listing_Standard.Begin(tabContentRect);
        if (listing_Standard.ListItemSelectable("LiSe.settings".Translate(), Color.yellow,
                out _, SelectedDef == "Settings"))
        {
            SelectedDef = SelectedDef == "Settings" ? null : "Settings";
        }

        listing_Standard.ListItemSelectable(null, Color.yellow, out _);
        foreach (var thingDef in allFacilities)
        {
            if (Main.HaveAnySavedSettings(thingDef.defName))
            {
                GUI.color = Color.green;
            }

            if (listing_Standard.ListItemSelectable(thingDef.label.CapitalizeFirst(), Color.yellow,
                    out var position,
                    SelectedDef == thingDef.defName))
            {
                SelectedDef = SelectedDef == thingDef.defName ? null : thingDef.defName;
            }

            GUI.color = Color.white;
            position.x = position.x + tabContentRect.width - iconSize.x;
            DrawIcon(thingDef, new Rect(position, iconSize));
        }

        listing_Standard.End();
        Widgets.EndScrollView();
    }
}