using System;
using System.Linq;
using EpicDodgeballBattle.Entities.Map;
using EpicDodgeballBattle.Entities.Weapons;
using EpicDodgeballBattle.Players;
using EpicDodgeballBattle.Players.Loadouts;
using EpicDodgeballBattle.Systems;
using EpicDodgeballBattle.Ui;
using Sandbox;

namespace EpicDodgeballBattle.Entities.Projectiles
{
	[Library( "db_balloon" )]
	public class BalloonProjectile : Prop, IUse, IHudEntity
	{
		public Team Team { get; set; }
		public Entity Attacker { get; set; }
		public EntityHud Hud { get; set; }
		public BalloonProjectileIndicator Indicator { get; set; }
		public Vector3 LocalCenter => CollisionBounds.Center;

		public override void Spawn()
		{
			SetModel( "models/prop_db_balloon/balloon.vmdl" );

			SetupPhysicsFromModel( PhysicsMotionType.Dynamic );

			Transmit = TransmitType.Always;
			
			base.Spawn();
		}

		public override void ClientSpawn()
		{
			Hud = new EntityHud()
			{
				Entity = this,
				UpOffset = 0f,
				MaxDistanceView = 300f
			};

			Indicator = Hud.AddChild<BalloonProjectileIndicator>();

			base.ClientSpawn();
		}

		[Event.Tick.Client]
		private void Tick()
		{
			float distance = Local.Pawn.Position.Distance(Position);
			float mapped = distance.Remap(Hud.MaxDistanceView, 0f).Clamp(0f, 1);

			if ( Equals( Hud.Style.Opacity, mapped ) )
			{
				return;
			}

			Hud.Style.Opacity = mapped;
			Hud.Style.Dirty();
		}

		public bool OnUse( Entity user )
		{
			if ( user is DodgeballPlayer player )
			{
				if ( Owner is null && Attacker is not null )
				{
					DodgeballPlayer? prisonerToRelease =
						Rounds.Current
							.Players
							.Where( p => p.Team == player.Team && p.Loadout is PrisonerLoadout )
							.OrderBy( _ => Guid.NewGuid() ).FirstOrDefault();
					
					if ( prisonerToRelease == null )
					{
						return false;
					}
					
					prisonerToRelease.GiveLoadout<PlayerLoadout>();
					
					PlayerSpawnPoint? teamSpawnPoint = Game.PlayerSpawnPoints
						.FirstOrDefault( psp => psp.Team == prisonerToRelease.Team && !psp.IsJail );

					if ( teamSpawnPoint != null )
						prisonerToRelease.Transform = teamSpawnPoint.Transform;
					else
						Log.Error( "Failed to find the jail spawn point on the map" );
				}

				if ( player.Inventory.Active is DodgeballWeapon )
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
			if ( eventData.Entity is not DodgeballPlayer )
				Attacker = null;

			if ( eventData.Entity is DodgeballPlayer targetPlayer
			     && Attacker is DodgeballPlayer attackerPlayer
			     && attackerPlayer.Team != targetPlayer.Team )
			{
				Attacker = null;

				Rounds.Current.OnPlayerIsPrisoner( targetPlayer, attackerPlayer );

				var damageInfo = DamageInfo.FromBullet(eventData.Pos, eventData.Velocity, 100)
					.WithAttacker(attackerPlayer)
					.WithWeapon(this);

				targetPlayer.TakeDamage(damageInfo);
			}

			base.OnPhysicsCollision( eventData );
		}

		protected override void OnDestroy()
		{
			if ( IsClient )
			{
				Hud.Delete();
			}
		}

		public bool IsUsable( Entity user )
		{
			return true;
		}
	}
}
