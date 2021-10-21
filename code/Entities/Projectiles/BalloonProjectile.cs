using System;
using EpicDodgeballBattle.Entities.Weapons;
using Sandbox;

namespace EpicDodgeballBattle.Projectiles
{
	[Library]
	public class BalloonProjectile : ModelEntity, IUse
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
				player.Inventory.Add( dbBalloon, true );
				Log.Info( "Use." );
				Delete();
			}

			return false;
		}

		public bool IsUsable( Entity user )
		{
			return true;
		}
	}
}
