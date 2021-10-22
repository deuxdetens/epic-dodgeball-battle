using System.Linq;
using EpicDodgeballBattle.Players;
using EpicDodgeballBattle.Players.Loadouts;
using Sandbox;

namespace EpicDodgeballBattle.Systems
{
	public class LobbyRound : BaseRound
	{
		protected override string RoundName => "LOBBY";

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
		
		public override void OnPlayerSpawn( DodgeballPlayer player )
		{
			player.Loadout?.Setup( player );

			base.OnPlayerSpawn( player );
		}

		public override void OnPlayerJoin( DodgeballPlayer player )
		{
			if ( Players.Contains( player ) )
			{
				return;
			}
			
			AddPlayer( player );
			
			player.Reset();
			player.SetTeam( Rand.Float() > 0.5f ? Team.Red : Team.Blue );
			player.GiveLoadout<LobbyLoadout>();
			player.Respawn();

			base.OnPlayerJoin( player );
		}
	}
}
