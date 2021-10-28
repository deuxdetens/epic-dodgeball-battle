using EpicDodgeballBattle.Entities;
using EpicDodgeballBattle.Players.Loadouts;
using EpicDodgeballBattle.Systems;
using EpicDodgeballBattle.Ui;
using Sandbox;

namespace EpicDodgeballBattle.Players
{
	[Library("epic_dodgeball_battle", Title = "Epic Dodgeball Battle")]
	public partial class DodgeballPlayer : Player, IHudEntity
	{
		public Vector3 LocalCenter => CollisionBounds.Center;
		public EntityHud Hud { get; set; }
		public Team JailTeam { get; set; }
		public readonly Clothing.Container Clothing;

		public DodgeballPlayer()
		{
			Inventory = new BaseInventory( this );
			Animator = new StandardPlayerAnimator();

			if(IsClient)
			{
				Hud = new EntityHud()
				{
					Entity = this,
					UpOffset = 50f,
					MaxDistanceView = 1000f
				};

				Hud.AddChild<PlayerIndicator>();
			}

			Clothing = new Clothing.Container();
		}

		public void MakePrisoner(Team jailTeam)
		{
			JailTeam = jailTeam;

			GiveLoadout<PrisonerLoadout>();
		}

		public override void Respawn()
		{
			base.Respawn();

			Rounds.Current?.OnPlayerSpawn( this );
		}

		public override void TakeDamage( DamageInfo info )
		{
			if ( LifeState == LifeState.Dead )
				return;

			base.TakeDamage( info );

			if(LifeState == LifeState.Dead)
				BecomeRagdollOnClient(Velocity, info.Flags, info.Position, info.Force, GetHitboxBone(info.HitboxIndex));
		}

		public void Reset()
		{
			Team = Team.None;
		}

		public override void Simulate( Client client )
		{
			SimulateActiveChild( client, ActiveChild );

			var targetWeapon = Input.ActiveChild as BaseWeapon;
			
			TickPlayerUse();

			var controller = GetActiveController();
			controller?.Simulate( client, this, GetActiveAnimator() );
		}

		protected override Entity FindUsable()
		{
			TraceResult trace = Trace.Ray( EyePos, EyePos + EyeRot.Forward * 110f )
				.HitLayer( CollisionLayer.Debris )
				.Radius( 2 )
				.Ignore( this )
				.Run();

			return !IsValidUseEntity( trace.Entity ) ? null : trace.Entity;
		}

		protected override void OnDestroy()
		{
			if(IsClient)
				Hud.Delete();

			base.OnDestroy();
		}
	}
}
