using Sandbox;

namespace EpicDodgeballBattle.Systems
{
	public class PrePlayRound : BaseRound
	{
		public override string RoundName => "PREPARE FOR THE BATTLE";

		public override int RoundDuration => 10;

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
