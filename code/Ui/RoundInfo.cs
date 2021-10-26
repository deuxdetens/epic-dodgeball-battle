using System;
using EpicDodgeballBattle.Systems;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace EpicDodgeballBattle.Ui
{
	public class RoundInfo : Panel
	{
		public Panel Container;
		
		public Label RoundName;
		public Label TimeLeft;


		public RoundInfo()
		{
			StyleSheet.Load( "/Ui/RoundInfo.scss" );

			Container = Add.Panel( "container" );
			RoundName = Container.Add.Label( "Round", "name" );
			TimeLeft = Container.Add.Label( "00:00", "timeleft" );
		}

		public override void Tick()
		{
			Entity player = Local.Pawn;
			if ( player == null )
			{
				return;
			}

			var game = Game.Current;
			if ( game == null )
			{
				return;
			}
			
			BaseRound round = Rounds.Current;
			if ( round == null )
			{
				return;
			}

			RoundName.Text = round.RoundName;

			if ( round.RoundDuration > 0 )
			{
				TimeLeft.Text = TimeSpan.FromSeconds( round.TimeLeftSeconds ).ToString( @"mm\:ss" );
				Container.SetClass( "roundNameOnly", false );
			}
			else
			{
				Container.SetClass( "roundNameOnly", true );
			}
			
			base.Tick();
		}
	}
}
