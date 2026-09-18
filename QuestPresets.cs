namespace QuestBypasser
{
    public static class QuestPresets
    {
        public static readonly (string Map, int QuestId)[] MapSpecific =
        {
            // ---- ultras / army bosses ----
            ("ultradage", 793),
            ("ultradarkon", 8733),
            ("ultradrago", 8395),
            ("ultraspeaker", 9125),
            ("ultradrakath", 3879),
            ("doomvault", 2954),
            ("doomvault", 3008),
            ("victormatsuri", 10294),   // Masakado
            ("astralshrine", 9802),     // Astral Empyrean
            ("astralshrine", 9803),
            ("novashrine", 9802),
            ("bocklincastle", 102520),
            ("bocklingrove", 10239),
            ("necroproject", 9901),

            // ---- former CoreArmyLite blanket list, mapped by the user ----
            ("wolfwing", 598),          // Wolfwing Evil End
            ("doomvaultb", 3004),       // Grim Underdungeon XXIX
            ("doomvaultb", 3008),       // I Command You, Help Me!
            ("towerofdoom10", 3484),    // Defeat Slugbutter
            ("finalbattle", 3799),      // Beat Death! (quest says shadowattack, used for finalbattle)
            ("mummies", 4616),          // removed from game's quest data; CruxShip.cs uses it here
            ("gluttony", 5915),         // Glutus, Take 2
            ("borgars", 7522),          // Burglinster's Revenge
            ("downbelow", 8107),        // removed from quest data; CoreBots "Bypass Banned" region
            ("manacradle", 9126),       // Once Upon Another Time
            ("liatarahill", 9814),      // Changeling

            // ---- story bosses / locked cells ----
            ("vath", 354),
            ("chaoscave", 567),
            ("chaoscave", 597),
            ("ledgermayne", 847),
            ("chaoslord", 3879),
            ("chaoslord", 3880),
            ("queenbattle", 8361),
            ("trygve", 8298),
            ("thunderfang", 1170),
            ("shadowattack", 3799),
            ("hakuwar", 9607),
            ("championdrakath", 2814),
            ("alteonbattle", 3824),

            // ---- farm / quest maps ----
            ("necrodungeon", 2059),
            ("battleunderc", 935),
            ("wanders", 976),
            ("wanders", 3773),
            ("titanattack", 8777),
            ("techfortress", 7646),
            ("stonewooddeep", 7650),
            ("blindingsnow", 899),
            ("mummies", 4614),
            ("pyramid", 4614),
            ("maloth", 6000),
            ("moonyardb", 1176),
            ("sandsea", 811),
            ("zorbaspalace", 7484),
            ("voidrefuge", 9531),
            ("darkoviagrave", 498),
            ("backroom", 8060),
            ("pyrewatch", 4077),
            ("astravia", 8000),
            ("firestorm", 1542),
            ("void", 904),
            ("rangda", 7622),
            ("starfest", 8094),
            ("badmoon", 9844),
            ("dawnfortress", 8297),
            ("dawnsanctum", 8297),
        };

        /// <summary>
        /// Gets the quest ID for the specified map, or null if not found in presets.
        /// </summary>
        public static int? GetQuestIdForMap(string mapName)
        {
            if (string.IsNullOrEmpty(mapName))
                return null;

            // Return the first matching quest for this map
            foreach (var (map, questId) in MapSpecific)
            {
                if (map.Equals(mapName, System.StringComparison.OrdinalIgnoreCase))
                    return questId;
            }

            return null;
        }
    }
}
