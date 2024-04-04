using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace ExampleLib.Content.Items {
    [AutoloadEquip(EquipType.Body)]
    public class RottenBreastplate : ModItem {
        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 28;
            Item.value = 2540;
            Item.rare = ItemRarityID.Red;
            Item.defense = 34;
        }

        public override void UpdateEquip(Player player)
        {
            player.statLifeMax2 = (int)(player.statLifeMax2 * 2f);
            player.statManaMax2 += 250;
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient<RottenCarapace>(50)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
        }
    }
}