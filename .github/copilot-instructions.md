# .github/copilot-instructions.md

## Mod Overview and Purpose

The RimWorld mod, titled "Linkable Settings," is designed to enhance the facility linking system within the game. The mod provides a more dynamic and flexible way for facilities to link with other structures, offering players additional strategic options for their colony management. The purpose of the mod is to improve the usability and customization of linked facilities in RimWorld, allowing players to optimize their space and resource management more effectively.

## Key Features and Systems

1. **Custom Linking Mechanics**: Introduces new mechanics for determining which facilities can link to one another based on user-defined criteria.
2. **Settings Interface**: Offers a settings interface within the game where players can tweak how facilities interact with each other.
3. **Dynamic Facility Validations**: Implements dynamic checks to validate potential facility links, making the system adaptable to various mod settings.
4. **Integration with Game's UI**: Seamlessly integrates with RimWorld's user interface for intuitive management of facility links.

## Coding Patterns and Conventions

- **Static Classes for Logic**: Utilizes static classes like `CompAffectedByFacilities_CanPotentiallyLinkTo_Static` and `CompAffectedByFacilities_IsPotentiallyValidFacilityForMe_Static` to encapsulate logic for facility linking, keeping the code modular and easy to maintain.
- **Encapsulation with Internal Classes**: Internal classes such as `LinkableSettingsMod` and `LinkableSettingsModSettings` encapsulate the mod-specific settings and UI logic, promoting separation of concerns.
- **Method Naming Conventions**: Methods are named clearly with a focus on their purpose, such as `DrawIcon`, `DrawOptions`, and `DrawTabsList` to manage UI drawing functionalities.

## XML Integration

Although specific XML files are not detailed in the summary, standard RimWorld modding typically involves defining new game definitions and settings through XML files. This includes:

- **Defs Folder**: To store custom ThingDefs, BuildingDefs, etc., enabling the game to recognize modded content.
- **XML Tagging**: Define new XML tags or extension points if needed for custom linking logic.

## Harmony Patching

Harmony is used to patch RimWorld’s base game methods to extend or modify existing functionality without altering the original game code. Here are potential usages within this mod:

- **Prefix and Postfix Methods**: Implement Harmony patches using prefix and postfix methods to inject custom logic before or after existing game methods.
- **Static Method Patching**: The use of static method patching is common for efficiently targeting methods like those in the `CompAffectedByFacilities` classes.

## Suggestions for Copilot

- **Code Completion**: Leverage Copilot to suggest code snippets for recurring UI patterns such as tab and button creation within the mod settings UI.
- **Harmony Patch Templates**: Use Copilot for generating template code for typical Harmony patches, making it easier to extend game functionalities.
- **XML Handling**: Provide code suggestions for reading and writing XML configuration files to streamline the integration process.
- **Optimization Tips**: Suggest performance optimizations, such as efficient data structures or algorithms, for handling facility link validations.
- **Documentation Assist**: Assist in generating inline documentation and comments to maintain clarity and readability of the code base.

This document serves as a guide for using GitHub Copilot effectively while developing the "Linkable Settings" mod for RimWorld in C#, ensuring a smooth and productive modding experience.
