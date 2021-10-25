using EpicDodgeballBattle.Entities.Projectiles;
using Sandbox;

namespace EpicDodgeballBattle.Entities.Weapons
{
	[Library("db_balloon_weapon", Title = "Dodgeball Balloon Weapon")]
	public partial class DodgeballWeapon : BaseWeapon
	{
		public Vector3 ShootFrom => Owner.EyePos;
		public Rotation ShootFromAngle => Owner.EyeRot;
		public float IneritVelocity => 1000f;
		public float ForceSpeed => 50f;
		[Net, Predicted]
		public float ProjectileForce { get; set; }

		public override void Simulate( Client player )
		{
			if(!IsServer)
				return;

			using ( Prediction.Off() )
			{
				if(Input.Pressed(InputButton.Attack1))
					ProjectileForce = 0;

				if(ProjectileForce < 100f && Input.Down(InputButton.Attack1))
					ProjectileForce += Time.Delta * ForceSpeed;

				if(Input.Released(InputButton.Attack1))
				{
					FireProjectile();

					if ( Owner.Inventory.Drop( this ) )
					{ 
						Delete();	
					}
				}
			}
		}

		private void FireProjectile()
		{
			_ = new BalloonProjectile
			{
				RenderColor = RenderColor,
				Position = ShootFrom + ShootFromAngle.Forward * 50f,
				Rotation = ShootFromAngle,
				Velocity = ShootFromAngle.Forward * IneritVelocity * (ProjectileForce / 100),
				Attacker = Owner
			};
		}
	}
}
