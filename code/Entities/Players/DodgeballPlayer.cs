using EpicDodgeballBattle.Entities.Weapons;
using Sandbox;

namespace EpicDodgeballBattle.Players
{
	[Library("epic-dodgeball-battle", Title = "Epic Dodgeball Battle")]
	public class DodgeballPlayer : Player
	{
		public DodgeballPlayer()
		{
			Inventory = new BaseInventory( this );
		}
		
		public override void Respawn()
		{
			SetModel( "models/citizen/citizen.vmdl" );

			Controller = new WalkController();

			Animator = new StandardPlayerAnimator();
			

			Camera = new FirstPersonCamera();

			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;

			DodgeballWeapon dbBalloon = Library.Create<DodgeballWeapon>( "db_balloon" );

			Inventory.Add( dbBalloon );
			ActiveChild = dbBalloon;
			base.Respawn();
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
