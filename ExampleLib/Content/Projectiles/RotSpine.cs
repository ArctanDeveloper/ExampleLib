

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace ExampleLib.Content.Projectiles {
    public class RotSpine : ModProjectile {
        public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 18;
			Projectile.timeLeft = 90;
			Projectile.penetrate = -1;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.netImportant = true;
			Projectile.aiStyle = -1;
		}

        public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
		}

        public override bool PreDraw(ref Color lightColor)
        {
			Vector2 draw = Projectile.Center - Main.screenPosition;
			for (int i = 0; i < 32; i++) Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)(draw.X + Main.rand.Next(-20, 20)), (int)(draw.Y + Main.rand.Next(-20, 20)), 1, 1), Color.DarkSeaGreen);
            return true;
        }
    }
}