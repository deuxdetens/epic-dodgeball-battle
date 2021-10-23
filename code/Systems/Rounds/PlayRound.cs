using System.Linq;
using EpicDodgeballBattle.Players;
using EpicDodgeballBattle.Players.Loadouts;
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
				var players = Client.All.Select( client => client.Pawn as DodgeballPlayer ).ToList();

				foreach ( DodgeballPlayer player in players )
				{
					SpawnPlayer( player );
				}
			}
		}
		
		public override void OnTick()
		{
			RedScore = Client.All.Select( client => client.Pawn as DodgeballPlayer )
				.Count(player => player.Team == Team.Blue && player.Loadout is PrisonerLoadout);
			
			BlueScore = Client.All.Select( client => client.Pawn as DodgeballPlayer )
				.Count(player => player.Team == Team.Red && player.Loadout is PrisonerLoadout);
			base.OnTick();
		}

		private void SpawnPlayer( DodgeballPlayer player )
		{
			if ( !Players.Contains( player ) )
				AddPlayer( player );

			player.Reset();
			player.SetTeam( Team.Red.GetCount() > Team.Blue.GetCount() ? Team.Blue : Team.Red );
			player.GiveLoadout<PlayerLoadout>();
			player.Respawn();
		}
	}
}
