# ScriptCommandPlus (SCP)

A Stardew Valley mod that allows Content Patcher (CP) events to trigger custom C# commands defined by mod developers. ScriptCommandPlus acts as a bridge between CP and the game's C# codebase, enabling powerful event-driven gameplay features such as NPC-initiated proposals, breakups, and divorces.

Currently, SCP registers three built-in commands: `propose`, `breakup`, and `divorce`.

## Features

* **NPC-Initiated Relationships**: NPCs can proactively propose to, break up with, or divorce the player, triggered by CP events.
* **Custom Command Bridge**: Register unlimited custom commands to be triggered by CP event scripts, with full support for parameter passing.
* **Seamless Content Patcher Integration**: Use bridge commands directly in Content Patcher events:

  * `/propose <NPC name>`
  * `/breakup <NPC name>`
  * `/divorce <NPC name>`

* **Non-intrusive and Conflict-Free**: Commands are registered in a way that avoids conflicts with other mods.

## Usage

Add these commands to your Content Patcher event scripts to trigger relationship events from NPCs to the player.  
No additional C# coding is required for built-in commands.

**Example Content Patcher event entry:**
```json
"1234567/f Leah": "speak Leah \"I want to spend my life with you.\"/propose Leah/end"
````

## Installation

1. Install [SMAPI] and [Content Patcher].
2. Download this mod and extract it into the `Mods` folder.
3. Launch the game through SMAPI.

## Compatibility

* Fully compatible with vanilla Stardew Valley relationship mechanics.
* Does **not currently support multi-spouse mods**.

## License

This project is licensed under the [MIT License](LICENSE).

---

Enjoy expanding your Stardew Valley experience with event-driven NPC interactions!

