using System.Linq;
using EpicDodgeballBattle.Players;
using EpicDodgeballBattle.Players.Loadouts;
using Sandbox;

namespace EpicDodgeballBattle.Systems
{
	public class LobbyRound : BaseRound
	{
		public override string RoundName => "LOBBY";

		protected override void OnStart()
		{
			if ( Host.IsServer )
			{
				var players = 
					Client.All.Select( client => client.Pawn as DodgeballPlayer );

				foreach ( DodgeballPlayer player in players )
				{
					OnPlayerJoin( player );
				}
			}
		}
	}
}
