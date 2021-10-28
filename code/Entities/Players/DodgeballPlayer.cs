using EpicDodgeballBattle.Entities;
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
		public readonly Clothing.Container Clothing;
		private DamageInfo LastDamage { get; set; }

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

		public override void Respawn()
		{	
			Rounds.Current?.OnPlayerSpawn( this );

			base.Respawn();
		}

		public override void TakeDamage( DamageInfo info )
		{
			LastDamage = info;

			base.TakeDamage( info );
		}

		public override void OnKilled()
		{
			BecomeRagdollOnClient(Velocity, LastDamage.Flags, LastDamage.Position, LastDamage.Force, GetHitboxBone(LastDamage.HitboxIndex));

			Camera = new SpectateRagdollCamera();
			Controller = null;

			EnableAllCollisions = false;
			EnableDrawing = false;

			Inventory.DeleteContents();

			Respawn();

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
