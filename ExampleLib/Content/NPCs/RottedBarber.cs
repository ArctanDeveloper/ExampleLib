using System;
using ExampleLib.Content.Items;
using ExampleLib.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace ExampleLib.Content.NPCs {
    public class RottedBarber : ModNPC {
        private enum ActionState 
		{ 
			Normal,
			Despawn
		}

		private enum SpellState
		{
			Rain,
            Circle,
            Walls,
            Debuff,
            Size
		}

		public ref float AI_State => ref NPC.ai[0];
		public ref float AI_SpellState => ref NPC.ai[1];
		public ref float AI_Timer => ref NPC.ai[2];

        public override void SetDefaults() {
			NPC.width = 64;
			NPC.height = 64;
			NPC.damage = 100;
			NPC.defense = 50;
			NPC.lifeMax = 50000;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath2;
			NPC.value = 6000f;
			NPC.knockBackResist = 0f;
			NPC.aiStyle = -1;
			NPC.noGravity = true;
			SceneEffectPriority = SceneEffectPriority.BossHigh;
            Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/Barber Of Decay");
			NPC.noTileCollide = true;
			NPC.boss = true;
		}

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            //This is loot.
            npcLoot.Add(ItemDropRule.OneFromOptions(1, new int[] {ItemID.StinkPotion, ModContent.ItemType<FetidTome>(), ModContent.ItemType<DecayAxe>(), ModContent.ItemType<SwordOfRot>()}));
            npcLoot.Add(ItemDropRule.Common(ItemID.FossilOre, 1, 35, 125));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<RottenCarapace>(), 2, 35, 55));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<RottenCarapace>(), 3, 15, 35));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<RottenCarapace>(), 5, 5, 25));
            npcLoot.Add(ItemDropRule.CoinsBasedOnNPCValue(Main.rand.Next(ModContent.NPCType<RottedBarber>())));
        }

        public override void AI()
		{
			NPC.TargetClosest(true);

            //Get the npc's target
			Player target = Main.player[NPC.target];
			
            //Handle NPC ai state
            switch (AI_State) {
                case (int)ActionState.Normal:
                    Vector2 desired_velocity = (target.Center - NPC.Center).SafeNormalize(Vector2.Zero) * 8;
                    Vector2 steering = desired_velocity - NPC.velocity;
                    steering = new Vector2(Math.Clamp(steering.X, -2, 2), Math.Clamp(steering.Y, -2, 2)) / 2;
                    NPC.velocity = NPC.velocity + steering;
                    NPC.velocity = new Vector2(Math.Clamp(NPC.velocity.X, -32, 32), Math.Clamp(NPC.velocity.Y, -32, 32));
                    
                    if (AI_Timer % (NPC.life < NPC.lifeMax / 8 ? 10 : 20) == 0 && NPC.life < NPC.lifeMax / 2) {
                        Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * 3, ProjectileID.DD2DarkMageBolt, NPC.damage, 0);
                        if (NPC.life < NPC.lifeMax / 4) {
                            for (int i = 0; i < 8; i++) Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center - NPC.velocity * 100 + Main.rand.NextVector2Circular(70, 70), (NPC.velocity * 4).RotatedByRandom(0.5f), ProjectileID.DD2DarkMageBolt, NPC.damage, 0);
                        
                            if (NPC.life < NPC.lifeMax / 10) {
                                for (int i = 0; i < 2; i++) {
                                    Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, (NPC.velocity * 8).RotatedByRandom(MathHelper.PiOver2), ProjectileID.DD2DarkMageBolt, NPC.damage * 2, 0);
                                }

                                if (NPC.life < NPC.lifeMax / 12) {
                                    for (int i = 0; i < 2; i++) {
                                        Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, (NPC.velocity * 12).RotatedByRandom(MathHelper.PiOver4), ProjectileID.DD2DarkMageBolt, NPC.damage * 4, 0);
                                    }
                                }
                            }
                        }
                    }
                    break;
            }

            //If boss is below 1 tenth health then if the ai timer is divisible by 50 cast a spell, other wise if the ai timer is divisible by 100 cast a spell.
            if (AI_Timer % (NPC.life < NPC.lifeMax / 10 ? 50 : 100) == 0) {
                switch (AI_SpellState) {
                    case (int)SpellState.Rain:
                        for (int i = 0; i < 24; i++) {
                            Vector2 spawnPos = target.Center + new Vector2(Main.rand.NextFloat(-800, 800), Main.rand.NextFloat(-800, -600));
                            Vector2 speed = new Vector2(Main.rand.NextFloat(-10, 10), Main.rand.NextFloat(10, 20));
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPos, speed, ModContent.ProjectileType<RotSpine>(), NPC.damage, 0);
                        }
                        break;
                    case (int)SpellState.Circle:
                        for (int i = 0; i < 16; i++) {
                            Vector2 spawnPos = target.Center + Main.rand.NextVector2CircularEdge(500, 500);
                            Vector2 speed = spawnPos.DirectionTo(target.Center) * Main.rand.NextFloat(5, 12);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPos, speed, ModContent.ProjectileType<RotSpine>(), NPC.damage, 0);
                        }
                        break;
                    case (int)SpellState.Walls:
                        for (int i = 0; i < 8; i++) {
                            Vector2 spawnPos = target.Center + Main.rand.NextVector2CircularEdge(500, 500);
                            Vector2 speed = spawnPos.DirectionTo(target.Center) * Main.rand.NextFloat(5, 12);
                            Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), spawnPos, speed, ModContent.ProjectileType<Tentacle>(), NPC.damage, 0).timeLeft = 120;
                        }

                        for (int i = 0; i < 8; i++) {
                            Vector2 spawnPos = target.Center + Main.rand.NextVector2CircularEdge(800, 800);
                            Vector2 speed = spawnPos.DirectionTo(target.Center) * Main.rand.NextFloat(8, 15);
                            Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), spawnPos, speed, ModContent.ProjectileType<Tentacle>(), NPC.damage, 0).timeLeft = 180;
                        }

                        for (int i = 0; i < 8; i++) {
                            Vector2 spawnPos = target.Center + Main.rand.NextVector2CircularEdge(1100, 1100);
                            Vector2 speed = spawnPos.DirectionTo(target.Center) * Main.rand.NextFloat(11, 18);
                            Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), spawnPos, speed, ModContent.ProjectileType<Tentacle>(), NPC.damage, 0).timeLeft = 240;
                        }
                        break;
                    case (int)SpellState.Debuff:
                        target.AddBuff(BuffID.Slow, 180);
                        NPC.Center = target.Center + Main.rand.NextVector2CircularEdge(1200, 1200);

                        //Summon the inner ring
                        for (int i = 0; i < 12; i++) {
                            Vector2 spawnPos = target.Center + Main.rand.NextVector2CircularEdge(500, 500); //Get the position
                            Vector2 speed = (spawnPos.DirectionTo(target.Center) * Main.rand.NextFloat(5, 12)).RotatedByRandom(MathHelper.PiOver4 * 0.2f); //Get the velocity to the target rotated by a bit
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPos, speed, ModContent.ProjectileType<RotSpine>(), NPC.damage, 0); //Summon the projectile
                        }

                        //Summon the middle ring
                        for (int i = 0; i < 12; i++) {
                            Vector2 spawnPos = target.Center + Main.rand.NextVector2CircularEdge(1200, 1200); //Get the position
                            Vector2 speed = (spawnPos.DirectionTo(target.Center) * Main.rand.NextFloat(15, 22)).RotatedByRandom(MathHelper.PiOver4 * 0.4f); //Get the velocity to the target rotated by a bit
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPos, speed, ModContent.ProjectileType<RotSpine>(), NPC.damage, 0); //Summon the projectile
                        }

                        //Summon the outer ring
                        for (int i = 0; i < 12; i++) {
                            Vector2 spawnPos = target.Center + Main.rand.NextVector2CircularEdge(1800, 1800); //Get the position
                            Vector2 speed = (spawnPos.DirectionTo(target.Center) * Main.rand.NextFloat(25, 32)).RotatedByRandom(MathHelper.PiOver4 * 0.6f); //Get the velocity to the target rotated by a bit
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPos, speed, ModContent.ProjectileType<RotSpine>(), NPC.damage, 0); //Summon the projectile
                        }

                        break;
                }

                AI_SpellState++; //Increment the spell state
                AI_SpellState %= (int)SpellState.Size; //Take the mod of the spell state to avoid going over the limit
            }

            AI_Timer++; //Increment the ai timer
		}

		public override void HitEffect(NPC.HitInfo hit) {
			for (int i = 0; i < 100; i++) {
				int dustType = Main.rand.Next(100, 200);
				int dustIndex = Dust.NewDust(NPC.position, NPC.width, NPC.height, dustType);
				Dust dust = Main.dust[dustIndex];
				dust.velocity.X += Main.rand.Next(-50, 51) * 0.01f;
				dust.velocity.Y += Main.rand.Next(-50, 51) * 0.01f;
				dust.scale *= 1f + Main.rand.Next(-30, 31) * 0.01f;
			}
		}
	}
}