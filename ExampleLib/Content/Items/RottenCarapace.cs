using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace ExampleLib.Content.Items {
    public class RottenCarapace : ModItem {
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 22;
            Item.maxStack = 999;
            Item.value = 2540;
            Item.rare = ItemRarityID.Red;
        }
    }
}