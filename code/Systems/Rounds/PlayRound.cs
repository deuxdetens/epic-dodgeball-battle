using System.Linq;
using EpicDodgeballBattle.Entities.Projectiles;
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

		protected override void OnStart()
		{
			if ( Host.IsServer )
			{
				DeleteBalloons();

				foreach(var balloonSpawnPoint in Game.BalloonSpawnPoints.ToList())
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
			if(attacker.IsValid())
			{
				player.MakePrisoner(attacker.Team);
				attacker.Client.AddInt("score");
			}

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
				DeleteBalloons();

				Rounds.Change( new StatsRound() );
			}
			else
			{
				ScoreHud?.Delete();
			}
		}

		private static void DeleteBalloons()
		{
			foreach(var entity in Entity.All.Where(e => e is BalloonProjectile))
					entity.Delete();
		}

		private void SpawnPlayer( DodgeballPlayer player )
		{
			if ( !Players.Contains( player ) )
				AddPlayer( player );

			player.Reset();
			player.SetTeam( Rand.Float() > 0.5f ? Team.Red : Team.Blue );
			if(Team.Blue.GetCount() + Team.Red.GetCount() > 1)
				if(Team.Blue.GetCount() == 0)
					player.SetTeam(Team.Blue);
				else if(Team.Red.GetCount() == 0)
					player.SetTeam(Team.Red);

			player.GiveLoadout<PlayerLoadout>();
			player.Respawn();
		}
	}
}
