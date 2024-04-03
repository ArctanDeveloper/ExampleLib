

using Microsoft.Xna.Framework;
using Terraria;
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
    }
}