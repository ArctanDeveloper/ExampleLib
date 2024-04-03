using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Audio;
using ExampleLib.Content.NPCs;

namespace ExampleLib.Content.Items {
    public class CodeSummon : ModItem {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("This is a summoning item use to summon a legendary being.");
            ItemID.Sets.SortingPriorityBossSpawns[Item.type] = 12;
        }

        public override void SetDefaults()
        {
            Item.width = 256;
            Item.height = 256;
            Item.maxStack = 999;
            Item.value = 10000;
            Item.rare = ItemRarityID.Red;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
        }

        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<RottedBarber>());
            SoundEngine.PlaySound(SoundID.Roar, player.position);
            return true;
        }
    }
}