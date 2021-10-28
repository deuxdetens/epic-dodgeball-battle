using Sandbox;
using Sandbox.UI;

namespace EpicDodgeballBattle.Ui
{
	public class Hud : HudEntity<RootPanel>
	{
		public Hud()
		{
			Host.AssertClient();
			
			RootPanel.StyleSheet.Load( "/Ui/Hud.scss" );
			RootPanel.AddChild<RoundInfo>();
			RootPanel.AddChild<RoundScore>();
			RootPanel.AddChild<BalloonIndicator>();
			RootPanel.AddChild<Scoreboard>();
			RootPanel.AddChild<ChatBox>();
			RootPanel.AddChild<KillFeed>();
			RootPanel.AddChild<PlayerIndicators>();
		}
	}
}
