using EpicDodgeballBattle.Lib.Network;
using Sandbox;

namespace EpicDodgeballBattle.Systems
{
	public partial class RoundGlobals : Globals
	{
		[Net, Change] public BaseRound Round { get; set; }
		
		public BaseRound LastRound { get; set; }
		
		private void OnRoundChanged()
		{
			if ( LastRound != Round )
			{
				LastRound?.Finish();
				LastRound = Round;
				LastRound.Start();
			}
		}

		[Event.Tick]
		private void Tick()
		{
			Round?.OnTick();
		}
	}

	public class Rounds
	{
		private static Globals<RoundGlobals> Variables => Globals.Define<RoundGlobals>( "rounds" );
		
		public static BaseRound Current => Variables.Value?.Round;

		public static void Change( BaseRound round )
		{
			Assert.NotNull( round );

			var entity = Variables.Value;

			if ( entity.IsValid() )
			{
				entity.Round?.Finish();
				entity.Round = round;
				entity.Round?.Start();
			}
		}
	}
}
