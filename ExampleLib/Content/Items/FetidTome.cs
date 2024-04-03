using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ExampleLib.Content.Items
{
    public class FetidTome : ModItem {
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 40;
            Item.maxStack = 1;
            Item.value = 10000;
            Item.rare = ItemRarityID.Red;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.mana = 10;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.damage = 60;
            Item.DamageType = DamageClass.MagicSummonHybrid;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 target = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
            for(int i = 0; i < (player.statLifeMax2 / 10) - (player.statLife / 10) + 4; i++) {
                Vector2 pos = target - new Vector2(Main.rand.NextFloat(-600, 600), Main.rand.NextFloat(600, 1200));
                Vector2 speed = pos.DirectionTo(target).RotatedByRandom(0.2f) * 12;
                Projectile.NewProjectile(source, pos, speed, ProjectileID.IceBolt, damage, knockback, player.whoAmI);
            }
            return false;
        }
    }
}