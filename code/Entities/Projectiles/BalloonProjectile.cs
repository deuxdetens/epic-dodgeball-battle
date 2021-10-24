﻿using EpicDodgeballBattle.Entities.Weapons;
using EpicDodgeballBattle.Players;
using EpicDodgeballBattle.Systems;
using Sandbox;

namespace EpicDodgeballBattle.Entities.Projectiles
{
	[Library("db_balloon")]
	public class BalloonProjectile : Prop, IUse
	{
		public Team Team { get; set; }

		public Entity Attacker { get; set; }

		public override void Spawn()
		{
			SetModel( "models/ball/ball.vmdl" );

			SetupPhysicsFromModel( PhysicsMotionType.Dynamic );

			Transmit = TransmitType.Always;

			base.Spawn();
		}

		public bool OnUse( Entity user )
		{
			if ( user is DodgeballPlayer player )
			{
				if(player.Inventory.Active is DodgeballWeapon)
					return false;

				DodgeballWeapon dbBalloon = Library.Create<DodgeballWeapon>( "db_balloon_weapon" );
				dbBalloon.RenderColor = player.Team.GetRenderColor();

				player.Inventory.Add( dbBalloon, true );
				Delete();
			}

			return false;
		}

		protected override void OnPhysicsCollision( CollisionEventData eventData )
		{
			if ( eventData.Entity is not DodgeballPlayer)
			{
				Attacker = null;
			}
			
			if ( eventData.Entity is DodgeballPlayer targetPlayer 
			     && Attacker is DodgeballPlayer attackerPlayer 
			     && attackerPlayer.Team != targetPlayer.Team )
			{
				Rounds.Current.OnPlayerIsPrisoner( targetPlayer, attackerPlayer );
			}
			
			base.OnPhysicsCollision( eventData );
		}

		public bool IsUsable( Entity user )
		{
			return true;
		}
	}
}
