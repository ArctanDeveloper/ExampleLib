using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace ExampleLib.Content.Items {
    [AutoloadEquip(EquipType.Head)]
    public class RottenHelm : ModItem {
        public static readonly int AdditiveGenericDamageBonus = 50;

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = 2540;
            Item.rare = ItemRarityID.Red;
            Item.defense = 18;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs) {
			return body.type == ModContent.ItemType<RottenBreastplate>() && legs.type == ModContent.ItemType<RottenGreaves>();
		}

		// UpdateArmorSet for bonuses
		public override void UpdateArmorSet(Player player) {
			player.setBonus = "A decaying suit gives you 50% generic increase in damage.";
			player.GetDamage(DamageClass.Generic) += AdditiveGenericDamageBonus / 100f; // Increase dealt damage for all weapon classes by 50%
            player.GetDamage(DamageClass.Magic) *= AdditiveGenericDamageBonus / 40f;// 1.25 times the amount of magic damage.
		}
        public override void UpdateEquip(Player player)
        {
            player.statLifeMax2 = (int)(player.statLifeMax2 * 1.5f);
            player.statManaMax2 += 100;
        }
        
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient<RottenCarapace>(25)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
        }
    }
}