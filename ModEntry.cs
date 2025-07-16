using StardewModdingAPI;
using StardewValley;

namespace ScriptCommandPlus
{
    public class ModEntry : Mod
    {
        public static CommandManager CommandManager = new();  
        public static IMonitor ModMonitor;

        public override void Entry(IModHelper helper)
        {
            ModMonitor = this.Monitor; 

            StardewValley.Event.RegisterCommand("propose", CustomCommands.ProposeCommand);
            StardewValley.Event.RegisterCommand("breakup", CustomCommands.BreakupCommand);
            StardewValley.Event.RegisterCommand("divorce", CustomCommands.DivorceCommand);

            //  Register a test console command
            helper.ConsoleCommands.Add("divorce", "Usage：divorce <NPC name>", OnScpTest);
            helper.ConsoleCommands.Add("breakup", "Usage: breakup <NPC name>", OnScpTest);
            helper.ConsoleCommands.Add("propose", "Usage: propose <NPC name>", OnScpTest);

            CommandManager.Register("propose", args =>
            {
                if (args.Length == 0)
                {
                    Monitor.Log("[ScriptCommandPlus] Usage: scptest propose <NPC名字>", LogLevel.Info);
                    return;
                }

                string npcName = args[0];
                NPC npc = Game1.getCharacterFromName(npcName);
                if (npc == null)
                {
                    Monitor.Log($"[ScriptCommandPlus] 没有找到NPC：{npcName}", LogLevel.Error);
                    return;
                }

                Farmer player = Game1.player;
                if (!player.friendshipData.TryGetValue(npcName, out Friendship friendship))
                    friendship = player.friendshipData[npcName] = new Friendship();

                friendship.Status = FriendshipStatus.Engaged;
                player.spouse = npcName;

                
                WorldDate weddingDate = new WorldDate(Game1.Date);
                weddingDate.TotalDays += 3;
                friendship.WeddingDate = weddingDate;

                player.changeFriendship(2500, npc);
                Game1.addMailForTomorrow("weddingInvite", noLetter: false, sendToEveryone: false);

                Monitor.Log($"[ScriptCommandPlus] ✅ You are now engaged to {npcName}. The wedding is scheduled for {weddingDate.Season} {weddingDate.DayOfMonth}!", LogLevel.Alert);
            });
            CommandManager.Register("divorce", args =>
            {
                if (args.Length == 0)
                {
                    Monitor.Log("[ScriptCommandPlus] Usage: divorce <NPC name>", LogLevel.Info);
                    return;
                }
                CustomCommands.DivorceCommand(null, new[] { "divorce", args[0] }, null);
            });
            CommandManager.Register("breakup", args =>
            {
                if (args.Length == 0)
                {
                    Monitor.Log("[ScriptCommandPlus] Usage: breakup <NPC name>", LogLevel.Info);
                    return;
                }
                CustomCommands.BreakupCommand(null, new[] { "breakup", args[0] }, null);
            });


            // Centralized handler for console commands
            void OnScpTest(string command, string[] args)
            {
                if (args.Length < 1)
                {
                    Game1.showRedMessage($"Usage: {command} <NPC name>");
                    return;
                }

                string npcName = args[0];

                // Route to specific logic
                switch (command.ToLower())
                {
                    case "divorce":
                        CustomCommands.DivorceCommand(null, new[] { "divorce", npcName }, null);
                        break;
                    case "breakup":
                        CustomCommands.BreakupCommand(null, new[] { "breakup", npcName }, null);
                        break;
                    case "propose":
                        CustomCommands.ProposeCommand(null, new[] { "propose", npcName }, null);
                        break;
                    default:
                        Game1.showRedMessage($"Unknown command: {command}");
                        break;
                }
            }

        }


        private void OnScpTest(string cmd, string[] args)
        {
            if (args.Length == 0)
            {
                Monitor.Log("Please specify the command name to test", LogLevel.Info);
                return;
            }

            string name = args[0];
            string[] rest = args.Skip(1).ToArray();

            bool ok = CommandManager.TryExecute(name, rest);
            if (!ok)
                Monitor.Log($"No command registered with the name {name}!", LogLevel.Warn);
        }
    }
}
