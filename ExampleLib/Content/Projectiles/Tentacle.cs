using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ReLogic.Content;
using Terraria.DataStructures;

namespace ExampleLib.Content.Projectiles
{
	public class Tentacle : ModProjectile
	{
		private const string ChainTexturePath = "ExampleLib/Content/Projectiles/TentacleChain";
		private const string ChainStartTexturePath = "ExampleLib/Content/Projectiles/TentacleChainStart";

		public ref float AI_Timer => ref Projectile.ai[0];

		public Vector2 startPos;

		public override void SetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 60;
			Projectile.timeLeft = 90;
			Projectile.penetrate = -1;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.netImportant = true;
			Projectile.aiStyle = -1;
			CooldownSlot = ImmunityCooldownID.Bosses;
		}

        public override void OnSpawn(IEntitySource source)
        {
			startPos = Projectile.Center;
        }

        public override void AI()
		{
			if (AI_Timer <= 60)
            {
				Projectile.velocity *= 1.01f;
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			}
			else
            {
				Projectile.velocity = Vector2.Zero;
            }

			AI_Timer++;
		}

        public override bool PreDraw(ref Color lightColor)
        {
			Asset<Texture2D> chainTexture = ModContent.Request<Texture2D>(ChainTexturePath);
			Asset<Texture2D> chainStartTexture = ModContent.Request<Texture2D>(ChainStartTexturePath);
			
			Rectangle? chainSourceRectangle = null;
			float chainHeightAdjustment = 0f;

			Vector2 chainOrigin = chainSourceRectangle.HasValue ? (chainSourceRectangle.Value.Size() / 2f) : (chainTexture.Size() / 2f);
			Vector2 chainDrawPosition = Projectile.Center;
			Vector2 vectorFromProjectileToPlayerArms = startPos.MoveTowards(chainDrawPosition, 4f) - chainDrawPosition;
			Vector2 unitVectorFromProjectileToPlayerArms = vectorFromProjectileToPlayerArms.SafeNormalize(Vector2.Zero);
			float chainSegmentLength = (chainSourceRectangle.HasValue ? chainSourceRectangle.Value.Height : chainTexture.Height()) + chainHeightAdjustment;
			if (chainSegmentLength == 0)
			{
				chainSegmentLength = 10;
			}
			float chainRotation = unitVectorFromProjectileToPlayerArms.ToRotation() + MathHelper.PiOver2;
			int chainCount = 0;
			float chainLengthRemainingToDraw = vectorFromProjectileToPlayerArms.Length() + chainSegmentLength / 2f;

			while (chainLengthRemainingToDraw > 0f)
			{
				Color chainDrawColor = Lighting.GetColor((int)chainDrawPosition.X / 16, (int)(chainDrawPosition.Y / 16f));

				var chainTextureToDraw = chainTexture;
				if (chainCount == 0)
				{
					chainTextureToDraw = chainStartTexture;
				}

				Main.spriteBatch.Draw(chainTextureToDraw.Value, chainDrawPosition - Main.screenPosition, chainSourceRectangle, chainDrawColor, chainRotation, chainOrigin, 1f, SpriteEffects.None, 0f);

				chainDrawPosition += unitVectorFromProjectileToPlayerArms * chainSegmentLength;
				chainCount++;
				chainLengthRemainingToDraw -= chainSegmentLength;
			}

			return true;
		}
    }
}