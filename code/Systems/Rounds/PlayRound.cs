using System.Linq;
using EpicDodgeballBattle.Players;
using EpicDodgeballBattle.Players.Loadouts;
using EpicDodgeballBattle.Ui;
using Sandbox;

namespace EpicDodgeballBattle.Systems
{
	public partial class PlayRound : BaseRound
	{
		public override string RoundName => "DODGEBALL BATTLE";

		public override int RoundDuration => 1200;

		public override bool ShowRoundInfo => true;
		
		public override bool ShowTimeLeft => true;
		
		[Net] public int BlueScore { get; set; }
		
		[Net] public int RedScore { get; set; }
		
		private RoundScore ScoreHud { get; set; }
		
		public override void OnPlayerJoin( DodgeballPlayer player )
		{
			SpawnPlayer( player );

			base.OnPlayerJoin( player );
		}

		public override void OnPlayerSpawn( DodgeballPlayer player )
		{
			base.OnPlayerSpawn( player );
			
			player.Loadout?.Setup( player );
		}

		protected override void OnStart()
		{
			if ( Host.IsServer )
			{
				foreach(var balloonSpawnPoint in Game.BalloonSpawnPoints)
					balloonSpawnPoint.Spawn();

				var players = Client.All.Select( client => client.Pawn as DodgeballPlayer ).ToList();

				foreach ( DodgeballPlayer player in players )
				{
					SpawnPlayer( player );
				}
			}
			else
			{
				ScoreHud = Local.Hud.AddChild<RoundScore>();
			}
		}

		public override void OnPlayerIsPrisoner( DodgeballPlayer player, DodgeballPlayer attacker )
		{
			player.GiveLoadout<PrisonerLoadout>();
			player.Loadout.Setup( player );

			var jailSpawnPoint = Game.PlayerSpawnPoints
				.FirstOrDefault( psp => psp.Team == attacker.Team && psp.IsJail );
			if(jailSpawnPoint != null)
				player.Transform = jailSpawnPoint.Transform;
			else
				Log.Error("Failed to find the jail spawn point on the map");

			ComputeTeamScore();

			int playerBlueCount = Players.Count( player => player.Team == Team.Blue && player.Loadout is PlayerLoadout );
			int playerRedCount = Players.Count( player => player.Team == Team.Red && player.Loadout is PlayerLoadout );

			if ( playerBlueCount == 0 || playerRedCount == 0 )
			{
				OnFinish();
			}
		}

		private void ComputeTeamScore()
		{
			RedScore = Players.Count(player => player.Team == Team.Blue && player.Loadout is PrisonerLoadout);
			BlueScore = Players.Count(player => player.Team == Team.Red && player.Loadout is PrisonerLoadout);
		}

		protected override void OnFinish()
		{
			if ( Host.IsServer )
			{
				foreach(var entity in Entity.FindAllByName("db_balloon"))
					entity.Delete();

				Rounds.Change( new StatsRound() );
			}
			else
			{
				ScoreHud?.Delete();
			}
		}

		private Entity FindFreeSpawnPoint(Team team) => Game.PlayerSpawnPoints
															.FirstOrDefault(psp => psp.Team == team && !psp.IsJail &&
																	!Players.Where(p => psp.Position.Distance(p.Position) < 20).Any());

		private void SpawnPlayer( DodgeballPlayer player )
		{
			if ( !Players.Contains( player ) )
				AddPlayer( player );

			player.Reset();
			player.SetTeam( Rand.Float() > 0.5f ? Team.Red : Team.Blue );
			player.GiveLoadout<PlayerLoadout>();
			player.Respawn();

			var spawnPoint = FindFreeSpawnPoint(player.Team);
			if(spawnPoint == null)
			{
				Log.Error("No free player spawn point found !");

				return;
			}

			player.Transform = spawnPoint.Transform;
		}
	}
}
