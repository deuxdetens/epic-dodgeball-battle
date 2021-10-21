using EpicDodgeballBattle.Players;
using EpicDodgeballBattle.Ui;
using Sandbox;

namespace EpicDodgeballBattle
{
	public class Game : Sandbox.Game 
	{
		public Hud Hud { get; set; }

		public static Game Instance
		{
			get => Current as Game;
		}
		
		[ServerVar( "edb_min_players", Help = "The minimum players required to start." )]
		public static int MinPlayers { get; set; } = 2;

		public Game()
		{
			if ( IsServer )
			{
				Hud = new Hud();
			}
		}
		
		public override void ClientJoined( Client client )
		{
			base.ClientJoined( client );

			DodgeballPlayer player = new();
			client.Pawn = player;
			player.Respawn();
		}
	}
}

