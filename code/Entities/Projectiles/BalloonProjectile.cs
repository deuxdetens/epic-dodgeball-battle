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
			var distance = Local.Pawn.Position.Distance(Position);
			var mapped = distance.Remap(Hud.MaxDistanceView, 0f).Clamp(0f, 1);

			if(Hud.Style.Opacity != mapped)
			{
				Hud.Style.Opacity = mapped;
				Hud.Style.Dirty();
			}
		}

		public bool OnUse( Entity user )
		{
			if ( user is DodgeballPlayer player )
			{
				if ( Owner is null && Attacker is not null )
				{
					var prisoners =
						Rounds.Current
							.Players
							.Where( p => p.Team == player.Team && p.Loadout is PrisonerLoadout )
							.OrderBy( x => Guid.NewGuid() ).FirstOrDefault();

					prisoners?.GiveLoadout<PlayerLoadout>();
					PlayerSpawnPoint? jailSpawnPoint = Game.PlayerSpawnPoints
						.FirstOrDefault( psp => psp.Team == prisoners?.Team && !psp.IsJail );

					if ( jailSpawnPoint != null )
						player.Transform = jailSpawnPoint.Transform;
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
			{
				Attacker = null;
			}

			if ( eventData.Entity is DodgeballPlayer targetPlayer
			     && Attacker is DodgeballPlayer attackerPlayer
			     && attackerPlayer.Team != targetPlayer.Team )
			{
				Attacker = null;
				DamageInfo damageInfo = new DamageInfo()
					.WithAttacker( attackerPlayer )
					.WithFlag( DamageFlags.PhysicsImpact )
					.WithForce( eventData.Velocity )
					.WithPosition( eventData.Pos )
					.WithWeapon( Owner );

				attackerPlayer.Client.AddInt("score");

				targetPlayer.BecomeRagdollOnClient( targetPlayer.Velocity, damageInfo.Flags, damageInfo.Position,
					damageInfo.Force, GetHitboxBone( damageInfo.HitboxIndex ) );

				Rounds.Current.OnPlayerIsPrisoner( targetPlayer, attackerPlayer );
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
