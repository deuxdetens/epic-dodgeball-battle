using Sandbox.UI;
using Sandbox.UI.Construct;

namespace EpicDodgeballBattle.Ui.World
{
	public class BalloonProjectileWorldPanel : WorldPanel
	{
		public Panel Container;
		
		public BalloonProjectileWorldPanel()
		{
			StyleSheet.Load( "/Ui/World/BalloonProjectileWorldPanel.scss" );
			Container = Add.Panel( "container" );
			Container.Add.Label( "Grab balloon: E", "use" );
		}
	}
}
