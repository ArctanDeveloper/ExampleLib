using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ExampleLib.Content.Items
{
    public class SwordOfRot : ModItem {
        public override void SetDefaults()
        {
            Item.width = 52;
            Item.height = 52;
            Item.value = 10000;
            Item.rare = ItemRarityID.Red;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 16;
            Item.useTime = 16;
            Item.damage = 110;
            Item.DamageType = DamageClass.Melee;
        }
    }
}