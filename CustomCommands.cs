using StardewModdingAPI;
using StardewValley;
using StardewValley.Network; 

namespace ScriptCommandPlus
{
    public static class CustomCommands
    {

        /// <summary>
        /// /propose <NPC name>
        /// Instantly engage the player to the specified NPC and schedule the wedding in 3 days.
        /// </summary>
        public static void ProposeCommand(Event evt, string[] args, EventContext ctx)
        {
            // Parameter check
            if (args.Length < 2)
            {
                Game1.showRedMessage("Usage：/propose <NPC name>");
                evt.CurrentCommand++;
                return;
            }

            string npcName = args[1];
            NPC npc = Game1.getCharacterFromName(npcName);
            if (npc == null)
            {
                Game1.showRedMessage($"Could not find an NPC named {npcName}.");
                evt.CurrentCommand++;
                return;
            }

            Farmer player = Game1.player;

            // Check if already engaged or married
            if (!string.IsNullOrEmpty(player.spouse))
            {
                Game1.showRedMessage($"You are already engaged to {player.spouse}, you can't accept another proposal!");
                evt.CurrentCommand++;
                return;
            }

            if (!player.friendshipData.TryGetValue(npcName, out Friendship friendship))
                friendship = player.friendshipData[npcName] = new Friendship();

            // Mark as engaged
            friendship.Status = FriendshipStatus.Engaged;
            player.spouse = npcName;

            // Schedule the wedding: today + 3 days
            WorldDate weddingDate = new WorldDate(Game1.Date);
            weddingDate.TotalDays += 3;
            friendship.WeddingDate = weddingDate;

            // Ensure friendship (2500 = 10 hearts)
            player.changeFriendship(2500, npc);

            // Remove old mail (for clean testing)
            player.mailReceived.Remove("weddingInvite");
            player.mailbox.Remove("weddingInvite");

            // Add wedding invitation mail
            Game1.addMailForTomorrow("weddingInvite", noLetter: false, sendToEveryone: false);

            // Log feedback
            ModEntry.ModMonitor.Log("Wedding invitation has been sent", LogLevel.Info);

            // Feedback
            string msg = $"You are now engaged to {npcName}. The wedding is scheduled for {weddingDate.Season} {weddingDate.DayOfMonth}！";
            Game1.showGlobalMessage(msg);

            evt.CurrentCommand++;
        }

        public static void BreakupCommand(Event evt, string[] args, EventContext ctx)
        {
            if (args.Length < 2)
            {
                Game1.showRedMessage("Usage：/breakup <NPC name>");
                if (evt != null) evt.CurrentCommand++;
                return;
            }

            string npcName = args[1];
            NPC npc = Game1.getCharacterFromName(npcName);
            if (npc == null)
            {
                Game1.showRedMessage($"Could not find an NPC named {npcName}.");
                if (evt != null) evt.CurrentCommand++;
                return;
            }

            Farmer player = Game1.player;
            if (!player.friendshipData.TryGetValue(npcName, out Friendship friendship) ||
                friendship.Status == FriendshipStatus.Friendly)
            {
                Game1.showRedMessage($"You are not dating, engaged to, or married to {npcName}.");
                if (evt != null) evt.CurrentCommand++;
                return;
            }

            // Remove relationship
            if (player.spouse == npcName)
                player.spouse = "";

            friendship.Status = FriendshipStatus.Friendly;
            // Optional: lower friendship to below 7 hearts
            friendship.Points = Math.Min(friendship.Points, 1749);

            Game1.showRedMessage($"You have already broken up with {npcName}.");
            if (evt != null) evt.CurrentCommand++;
        }


        public static void DivorceCommand(Event evt, string[] args, EventContext ctx)
        {
            if (args.Length < 2)
            {
                Game1.showRedMessage("Usage：/divorce <NPC name>");
                evt.CurrentCommand++;
                return;
            }

            string npcName = args[1];
            NPC npc = Game1.getCharacterFromName(npcName);
            if (npc == null)
            {
                Game1.showRedMessage($"Could not find an NPC named {npcName}.");
                evt.CurrentCommand++;
                return;
            }

            Farmer player = Game1.player;

            // Check if the player is currently married to this NPC
            if (player.spouse != npcName)
            {
                Game1.showRedMessage($"You can't divorce {npcName} because they are not your spouse.");
                evt.CurrentCommand++;
                return;
            }

            // Friendship data
            Friendship friendship;
            if (!player.friendshipData.TryGetValue(npcName, out friendship))
            {
                Game1.showRedMessage($"No relationship data found for {npcName}.");
                evt.CurrentCommand++;
                return;
            }

            // Already divorced check
            if (friendship.IsDivorced())
            {
                Game1.showRedMessage($"You have already divorced {npcName}.");
                evt.CurrentCommand++;
                return;
            }

            // Official doDivorce core process (restored logic)

            // 1. Remove marriage-related active events
            player.removeMarriageActiveDialogueEvents(npcName);

            // 2. If not a roommate marriage, generate divorce event
            if (!npc.isRoommate())
                player.autoGenerateActiveDialogueEvent("divorced_" + npcName, 4);

            // 3. Clear spouse fields
            player.spouse = "";
            Game1.MasterPlayer.spouse = null;
            Game1.getFarm().UpdatePatio();

            // 4. Remove special item (ID=460, wedding ring)
            player.specialItems.RemoveWhere((string id) => id == "460");

            // 5. Set friendship data to Divorced, separated, split up
            friendship.Points = 0;
            friendship.RoommateMarriage = false;
            friendship.Status = FriendshipStatus.Divorced;

            // 6. Reset NPC divorce status (return to default map)
            npc.PerformDivorce();

            // 7. emove spouse's room in the house
            Utility.getHomeOfFarmer(player).playerDivorced();

            // 8. Refresh farmhouse patio
            Game1.getFarm().UpdatePatio();

            // 9. Remove wedding quest
            player.removeQuest("126");

            Game1.showRedMessage($"You and {npcName} are now divorced.");

            evt.CurrentCommand++;
        }
    }
}
