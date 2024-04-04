using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace ExampleLib.Content.Items {
    [AutoloadEquip(EquipType.Legs)]
    public class RottenGreaves : ModItem {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = 2540;
            Item.rare = ItemRarityID.Red;
            Item.defense = 18;
        }

        public override void UpdateEquip(Player player)
        {
            player.statLifeMax2 = (int)(player.statLifeMax2 * 1.2f);
            player.statManaMax2 += 150;
        }
        
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient<RottenCarapace>(25)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
        }
    }
}