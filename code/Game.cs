using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EpicDodgeballBattle.Entities.Map;
using EpicDodgeballBattle.Players;
using EpicDodgeballBattle.Systems;
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
		public static IEnumerable<PlayerSpawnPoint> PlayerSpawnPoints => All.Where(e => e is PlayerSpawnPoint)
																			.Cast<PlayerSpawnPoint>();
		public static IEnumerable<BalloonSpawnPoint> BalloonSpawnPoints => All.Where(e => e is BalloonSpawnPoint)
																				.Cast<BalloonSpawnPoint>();

		public Game()
		{
			if ( IsServer )
			{
				Hud = new Hud();
			}
		}
		
		public async Task StartSecondTimer()
		{
			while (true)
			{
				await Task.DelaySeconds( 1 );
				await OnSecond();
			}
		}
		
		private Task OnSecond()
		{
			return CheckMinimumPlayers();
		}

		public override void PostLevelLoaded()
		{
			_ = StartSecondTimer();
			
			base.PostLevelLoaded();
		}
		
		[Event.Entity.PostSpawn]
		private void OnEntityPostSpawn()
		{
			if ( IsServer )
			{
				Rounds.Change( new LobbyRound() );
			}
		}
		
		[Event.Tick]
		private void OnTick()
		{
			Rounds.Current?.OnTick();
		}
		
		private async Task CheckMinimumPlayers()
		{
			if ( Client.All.Count >= MinPlayers )
			{
				if ( Rounds.Current is LobbyRound round|| Rounds.Current == null )
				{
					Rounds.Change( new PrePlayRound() );
				}
			}
			else if ( Rounds.Current is not LobbyRound )
			{
				Rounds.Change( new LobbyRound() );
			}
		}

		public override void ClientJoined( Client client )
		{
			base.ClientJoined( client );

			DodgeballPlayer player = new();
			client.Pawn = player;
			
			Rounds.Current.OnPlayerJoin( player );
		}
		
		public override void ClientDisconnect( Client client, NetworkDisconnectionReason reason )
		{
			Rounds.Current?.OnPlayerLeave( client.Pawn as DodgeballPlayer );

			base.ClientDisconnect( client, reason );
		}
	}
}

