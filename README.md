# ScriptCommandPlus
Stardew Valley mod — Adds bridge commands for NPC-initiated relationships via Content Patcher events.
# ScriptCommandPlus (SCP)

A Stardew Valley mod that provides custom bridge commands enabling NPC-initiated relationships (proposal, breakup, and divorce) through Content Patcher events. This mod serves as a bridge connecting Content Patcher (CP) and the game's C# codebase, allowing seamless event-driven relationship changes initiated by NPCs.

## Features

* **NPC-Initiated Relationships**: NPCs can proactively propose, break up, or divorce the player based on events you define.
* **Content Patcher Compatibility**: Easily trigger relationship commands directly within Content Patcher events using provided commands:

  * `/propose <NPC name>`
  * `/breakup <NPC name>`
  * `/divorce <NPC name>`

  **Supports calling custom C# methods from event scripts, with parameter passing.**

  **Supports registering unlimited commands in a non-intrusive, conflict-free way.**

## Usage

Simply include these commands in your Content Patcher event scripts to initiate relationship changes from NPCs to players seamlessly. No additional coding is required.

Example usage in a Content Patcher event:

```json
{
  "Action": "RunCommand",
  "Command": "/propose Abigail"
}
```

## Installation

1. Install [SMAPI](https://smapi.io/) and [Content Patcher](https://www.nexusmods.com/stardewvalley/mods/1915).
2. Download this mod and extract it into the `Mods` folder.
3. Launch the game through SMAPI.

## Compatibility

* Fully compatible with vanilla Stardew Valley relationship mechanics.
* Does **not currently support multi-spouse mods**.

## License

This project is licensed under the [MIT License](LICENSE).

---

Enjoy enhancing your Stardew Valley NPC relationships!
