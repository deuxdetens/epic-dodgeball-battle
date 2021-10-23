using Sandbox;
using Sandbox.UI;

namespace EpicDodgeballBattle.Ui
{
	public class Hud : HudEntity<RootPanel>
	{
		public Hud()
		{
			if ( !IsClient )
			{
				return;
			}
			
			RootPanel.StyleSheet.Load( "/Ui/hud.scss" );
			RootPanel.AddChild<RoundInfo>();
			RootPanel.AddChild<RoundScore>();
			RootPanel.AddChild<BalloonIndicator>();
		}
	}
}
