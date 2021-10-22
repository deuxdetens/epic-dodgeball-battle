using EpicDodgeballBattle.Entities.Weapons;
using EpicDodgeballBattle.Systems;
using Sandbox;

namespace EpicDodgeballBattle.Players
{
	[Library("epic-dodgeball-battle", Title = "Epic Dodgeball Battle")]
	public partial class DodgeballPlayer : Player
	{
		public DodgeballPlayer()
		{
			Inventory = new BaseInventory( this );
			Animator = new StandardPlayerAnimator();
		}

		public override void Respawn()
		{
			base.Respawn();
			
			Rounds.Current?.OnPlayerSpawn( this );
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
	}
}
