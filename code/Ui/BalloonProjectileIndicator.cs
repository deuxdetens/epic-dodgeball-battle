using Sandbox.UI;
using Sandbox.UI.Construct;

namespace EpicDodgeballBattle.Ui
{
	public class BalloonProjectileIndicator : Panel
	{
		private Panel Container;

		public BalloonProjectileIndicator()
		{
			StyleSheet.Load( "/Ui/BalloonProjectileIndicator.scss" );

			Container = Add.Panel("container");
			Container.Add.Label( "Grab balloon: E", "use" );
		}
	}
}
