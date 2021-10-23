using EpicDodgeballBattle.Entities.Weapons;
using EpicDodgeballBattle.Players;
using Sandbox;

namespace EpicDodgeballBattle.Entities.Projectiles
{
	[Library]
	public class BalloonProjectile : Prop, IUse
	{
		public override void Spawn()
		{
			SetModel( "models/ball/ball.vmdl" );

			SetupPhysicsFromModel( PhysicsMotionType.Dynamic );

			Transmit = TransmitType.Always;

			base.Spawn();
		}

		public bool OnUse( Entity user )
		{
			if ( user is Player player )
			{
				if(player.Inventory.Active is DodgeballWeapon)
					return false;

				var dbBalloon = Library.Create<DodgeballWeapon>( "db_balloon" );
				dbBalloon.RenderColor = RenderColor;

				player.Inventory.Add( dbBalloon, true );
				Log.Info( "Use." );
				Delete();
			}

			return false;
		}

		protected override void OnPhysicsCollision( CollisionEventData eventData )
		{
			if ( eventData.Entity is DodgeballPlayer player )
			{
				player.TakeDamage( DamageInfo.Generic( 1000f ) );
			}
			
			base.OnPhysicsCollision( eventData );
		}

		public bool IsUsable( Entity user )
		{
			return true;
		}
	}
}
