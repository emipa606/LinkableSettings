# GitHub Copilot Instructions for Linkable Settings Mod

## Mod Overview and Purpose

**Linkable Settings** is a RimWorld mod that allows players to change the linkable facilities in the game. This mod provides options to adjust the link range, link amount, and link type. It also introduces a new link-type called "Room," which allows facilities to link to any facility within the same room. Additionally, this mod offers the option to link through walls when the ranged option is selected.

**Author:** Mlie

**Package ID:** Mlie.LinkableSettings

**Dependencies:** brrainz.harmony

## Key Features and Systems

### Linkable Facilities

- **Link Range:** Allows players to set the range within which facilities can be linked.
- **Link Amount:** Controls the number of facilities that can be linked.
- **Link Type:** Provides different types of linking, including the new "Room" link type.

### Room Link Type

- **Room Link:** Allows facilities to link to any facility within the same room.
- **Range Link:** If a facility is outside a room, it will use the defined range for linking.

### Wall Linking

- **Wall Linking:** When the ranged link option is selected, players can choose to link through walls.

## Coding Patterns and Conventions

### C# Files

The C# files in the mod follow the standard RimWorld coding conventions. Here is a breakdown of each C# file:

- **CompAffectedByFacilities_CanPotentiallyLinkTo_Static.cs:** Contains a prefix method that modifies the linkable facilities.
- **CompAffectedByFacilities_IsPotentiallyValidFacilityForMe_Static.cs:** Contains a prefix method that validates the linkable facilities.
- **LinkableSettingsModSettings.cs:** Manages the mod settings and provides the `ExposeData` method for saving and loading settings.
- **LinkableSettingsMod.cs:** The main mod class that handles the mod settings window and other functionalities.
- **ListingExtension.cs:** Provides an extension method for the `Listing_Standard` class to add selectable list items.

### XML Integration

The mod uses XML files to define its settings and update information. The XML files are located in the `Defs` folder and follow the standard RimWorld XML conventions.

- **UpdateInfo.xml:** Contains the update information for the mod.

## Harmony Patching

The mod uses Harmony for patching the game's code. Harmony allows developers to modify existing methods without altering the original codebase. The mod uses Harmony patches in the following C# files:

- **CompAffectedByFacilities_CanPotentiallyLinkTo_Static.cs:** Contains a prefix method to modify the linkable facilities.
- **CompAffectedByFacilities_IsPotentiallyValidFacilityForMe_Static.cs:** Contains a prefix method to validate the linkable facilities.

## Suggestions for Copilot

### General

- When generating code, please use the standard RimWorld coding conventions.
- Ensure that any generated code follows the mod's coding patterns and conventions.

### XML Integration

- When generating XML files, please ensure that they follow the standard RimWorld XML conventions.
- The generated XML files should be located in the `Defs` folder and should be named appropriately.

### Harmony Patching

- When generating Harmony patches, please ensure that they are correctly integrated into the mod's code.
- The generated Harmony patches should be located in the appropriate C# files and should be named appropriately.

### C# Files

- When generating C# files, please ensure that they follow the standard RimWorld coding conventions.
- The generated C# files should be located in the appropriate folder and should be named appropriately.

## Conclusion

The Linkable Settings mod provides players with more control over the linkable facilities in RimWorld. By following the coding patterns and conventions outlined in this document, developers can contribute to the mod and ensure that their code is integrated smoothly into the mod's codebase.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

