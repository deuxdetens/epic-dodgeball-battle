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
		private readonly Hud Hud;
		[ServerVar( "edb_min_players", Help = "The minimum players required to start." )]
		public static int MinPlayers { get; set; } = 2;
		public static IEnumerable<PlayerSpawnPoint> PlayerSpawnPoints => All.Where(e => e is PlayerSpawnPoint)
																			.Cast<PlayerSpawnPoint>();
		public static IEnumerable<BalloonSpawnPoint> BalloonSpawnPoints => All.Where(e => e is BalloonSpawnPoint)
																				.Cast<BalloonSpawnPoint>();

		public Game()
		{
			if ( IsClient )
				Hud = new Hud();
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

			if ( IsServer )
			{
				Rounds.Change( new LobbyRound() );
			}
			
			base.PostLevelLoaded();
		}

		[Event.Tick]
		private void OnTick()
		{
			Rounds.Current?.OnTick();
		}

		[ServerCmd("restart_round")]
		public static void RestartRound()
		{
			if ( Rounds.Current is not PlayRound )
			{
				Log.Error("Current round is not PlayRound");
				return;
			}

			Rounds.Change( new PrePlayRound() );
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

			await Task.CompletedTask;
		}

		public override void ClientJoined( Client client )
		{
			base.ClientJoined( client );

			var player = new DodgeballPlayer();
			client.Pawn = player;
			
			Rounds.Current.OnPlayerJoin( player );
		}

		public override void OnKilled( Client client, Entity pawn )
		{
			base.OnKilled(client, pawn);

			Rounds.Current?.OnPlayerKilled(pawn as DodgeballPlayer);
		}
		
		public override void MoveToSpawnpoint(Entity pawn)
		{
			var player = pawn as DodgeballPlayer;

			if(player.IsPrisoner)
			{
				var jailSpawnPoint = PlayerSpawnPoints
					.FirstOrDefault( psp => psp.Team == player.JailTeam && psp.IsJail );
				if(jailSpawnPoint != null)
					player.Transform = jailSpawnPoint.Transform;
				else
					Log.Error("Failed to find the jail spawn point on the map");
			}
			else if(player.IsPlayer)
			{
				var spawnPoint = PlayerSpawnPoints
					.FirstOrDefault(psp => psp.Team == player.Team && !psp.IsJail &&
							!Rounds.Current.Players.Where(p => psp.Position.Distance(p.Position) < 20).Any());
				if(spawnPoint != null)
					player.Transform = spawnPoint.Transform;
				else
					Log.Error("No free player spawn point found !");
			}
			else
				base.MoveToSpawnpoint(pawn);
		}

		public override void ClientDisconnect( Client client, NetworkDisconnectionReason reason )
		{
			Rounds.Current?.OnPlayerLeave( client.Pawn as DodgeballPlayer );

			base.ClientDisconnect( client, reason );
		}

		protected override void OnDestroy()
		{
			Hud?.Delete();

			base.OnDestroy();
		}
	}
}

