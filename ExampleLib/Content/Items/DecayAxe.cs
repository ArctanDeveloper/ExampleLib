using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ExampleLib.Content.Items
{
    public class DecayAxe : ModItem {
        public override void SetDefaults()
        {
            Item.width = 68;
            Item.height = 68;
            Item.value = 10000;
            Item.rare = ItemRarityID.Red;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 40;
            Item.useTime = 40;
            Item.damage = 230;
            Item.DamageType = DamageClass.Melee;
        }
    }
}