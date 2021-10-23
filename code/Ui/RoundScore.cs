using EpicDodgeballBattle.Systems;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace EpicDodgeballBattle.Ui
{
	public class RoundScoreItem : Panel
	{
		public Label Score;

		public RoundScoreItem()
		{
			Score = Add.Label( "0", "score" );
		}
	}
	
	public class RoundScore : Panel
	{
		public Panel Container;
		public RoundScoreItem Blue;  
		public RoundScoreItem Red;  

		public RoundScore()
		{
			StyleSheet.Load( "/ui/RoundScore.scss" );

			Container = Add.Panel( "container" );
			Blue = Container.AddChild<RoundScoreItem>( "blue" );
			Red = Container.AddChild<RoundScoreItem>( "red" );
		}

		public override void Tick()
		{
			Entity player = Local.Pawn;
			if ( player == null ) return;

			Game game = Game.Instance;
			if ( game == null ) return;

			if ( Rounds.Current is PlayRound round )
			{
				Blue.Score.Text = round.BlueScore.ToString();
				Red.Score.Text = round.RedScore.ToString();
			}

			base.Tick();
		}
	}
}
