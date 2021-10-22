using EpicDodgeballBattle.Entities.Weapons;
using Sandbox;

namespace EpicDodgeballBattle.Players.Loadouts
{
	public class PlayerLoadout : BaseLoadout
	{
		public override void Setup( DodgeballPlayer player )
		{
			base.Setup( player );
			
			DodgeballWeapon dbBalloon = Library.Create<DodgeballWeapon>( "db_balloon" );
			
			player.Inventory.Add( dbBalloon );
			player.ActiveChild = dbBalloon;
		}
	}
}
