using Sandbox;

namespace EpicDodgeballBattle.Systems
{
	public class StatsRound : BaseRound
	{
		public override int RoundDuration => 10;
		
		protected override void OnStart()
		{
			
		}

		protected override void OnFinish()
		{
			if ( Host.IsServer )
			{
				Rounds.Change( new PlayRound() );
			}
		}

		protected override void OnTimeUp()
		{
			Finish();

			base.OnTimeUp();
		}
	}
}
