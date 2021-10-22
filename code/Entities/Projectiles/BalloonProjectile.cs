using System;
using EpicDodgeballBattle.Entities.Weapons;
using EpicDodgeballBattle.Players;
using Sandbox;

namespace EpicDodgeballBattle.Projectiles
{
	[Library]
	public class BalloonProjectile : Prop, IUse
	{
		public void Initialize()
		{
			Transmit = TransmitType.Always;

			SetupPhysicsFromModel( PhysicsMotionType.Dynamic );
		}

		public bool OnUse( Entity user )
		{
			if ( user is Player player )
			{
				DodgeballWeapon dbBalloon = Library.Create<DodgeballWeapon>( "db_balloon" );
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
