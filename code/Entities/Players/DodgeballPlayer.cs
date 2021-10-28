using EpicDodgeballBattle.Entities;
using EpicDodgeballBattle.Players.Loadouts;
using EpicDodgeballBattle.Systems;
using EpicDodgeballBattle.Ui;
using Sandbox;

namespace EpicDodgeballBattle.Players
{
	[Library("epic_dodgeball_battle", Title = "Epic Dodgeball Battle")]
	public partial class DodgeballPlayer : Player
	{
		public Team JailTeam { get; set; }
		public DamageInfo LastDamage { get; set; }
		public readonly Clothing.Container Clothing;

		public DodgeballPlayer()
		{
			Inventory = new BaseInventory( this );
			Animator = new StandardPlayerAnimator();

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

			LastDamage = info;

			base.TakeDamage( info );
		}

		public override void OnKilled()
		{
			BecomeRagdollOnClient(Velocity, LastDamage.Flags, LastDamage.Position,
									LastDamage.Force, GetHitboxBone(LastDamage.HitboxIndex));

			base.OnKilled();
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

			base.Simulate(client);
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
	}
}
