using System;
using EpicDodgeballBattle.Projectiles;
using Sandbox;

namespace EpicDodgeballBattle.Entities.Weapons
{
	[Library("db_balloon", Title = "Dodgeball Balloon")]
	public class DodgeballWeapon : BaseWeapon
	{
		public Vector3 ShootFrom => Owner.EyePos;
		public Rotation ShootFromAngle => Owner.EyeRot;
		
		public float BalloonRange => 20000f;
		
		public float Damage => 100f;

		public float IneritVelocity => 1000f;
		
		public float ProjectileForce => 0f;

		public override void AttackPrimary()
		{
			float spread = 1.5f;
			ShootBalloon( spread );

			if ( !IsServer )
			{
				return;
			}

			using ( Prediction.Off() )
			{
				FireProjectile();
					
				if ( Owner.Inventory.Drop( this ) )
				{
					Delete();	
				}
			}
		}

		private void FireProjectile()
		{
			BalloonProjectile projectile = new();
			projectile.SetModel( "models/ball/ball.vmdl" );
			projectile.RenderColor = Color.Red;
			projectile.Position = ShootFrom + ShootFromAngle.Forward * 50f;
			projectile.Rotation = ShootFromAngle;
			projectile.Initialize();
			projectile.Velocity = ShootFromAngle.Forward * IneritVelocity;
			projectile.PhysicsBody.ApplyForce( ShootFromAngle.Forward * ProjectileForce * 200f );
		}

		private void ShootBalloon( float spread )
		{
			Vector3 forward = Owner.EyeRot.Forward;
			forward += (Vector3.Random + Vector3.Random + Vector3.Random + Vector3.Random) * spread * 0.25f;
			forward = forward.Normal;

			foreach (TraceResult trace in TraceBullet(Owner.EyePos, Owner.EyePos + forward + BalloonRange))
			{
				trace.Surface.DoBulletImpact( trace );

				if ( !trace.Entity.IsValid() )
				{
					continue;
				}

				using ( Prediction.Off() )
				{
					DamageInfo damageInfo = new DamageInfo()
						.WithPosition( trace.EndPos )
						.WithFlag( DamageFlags.Bullet )
						.WithForce( forward * 100f * 1.5f )
						.UsingTraceResult( trace )
						.WithAttacker( Owner )
						.WithWeapon( this );

					damageInfo.Damage = GetDamageFalloff( trace.Distance, Damage, 0f, 0f );
					Log.Info( damageInfo.Damage );
					trace.Entity.TakeDamage( damageInfo );
				}
			}
		}
		
		private static float GetDamageFalloff( float distance, float damage, float start, float end )
		{
			if ( !(end > 0f) )
			{
				return damage;
			}

			if ( !(start > 0f) )
			{
				return Math.Max( damage - (damage / end) * distance, 0f );
			}

			if ( distance < start )
			{
				return damage;
			}

			float falloffRange = end - start;
			float difference = (distance - start);

			return Math.Max( damage - (damage / falloffRange) * difference, 0f );

		}
	}
}
