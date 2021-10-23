using EpicDodgeballBattle.Entities.Projectiles;
using Sandbox;

namespace EpicDodgeballBattle.Entities.Weapons
{
	[Library("db_balloon_weapon", Title = "Dodgeball Balloon Weapon")]
	public class DodgeballWeapon : BaseWeapon
	{
		public Vector3 ShootFrom => Owner.EyePos;
		public Rotation ShootFromAngle => Owner.EyeRot;

		public float IneritVelocity => 1000f;
		
		public float ProjectileForce => 0f;

		public override void AttackPrimary()
		{
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
			var projectile = new BalloonProjectile
			{
				RenderColor = RenderColor,
				Position = ShootFrom + ShootFromAngle.Forward * 50f,
				Rotation = ShootFromAngle,
				Velocity = ShootFromAngle.Forward * IneritVelocity,
				Attacker = Owner
			};
			projectile.PhysicsBody.ApplyForce( ShootFromAngle.Forward * ProjectileForce * 200f );
		}
	}
}
