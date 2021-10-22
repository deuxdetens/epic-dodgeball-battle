using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace EpicDodgeballBattle.Ui
{
	public class RoundInfo : Panel
	{
		public Panel Container;
		
		public Label RoundName;


		public RoundInfo()
		{
			StyleSheet.Load( "/ui/RoundInfo.scss" );

			Container = Add.Panel( "container" );
			RoundName = Container.Add.Label( "Round", "name" );
		}

		public override void Tick()
		{
			Entity player = Local.Pawn;
			if ( player == null )
			{
				return;
			}

			Game game = Game.Instance;
			if ( game == null )
			{
				return;
			}
			
			base.Tick();
		}
	}
}
