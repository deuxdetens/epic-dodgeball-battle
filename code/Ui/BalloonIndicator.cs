using Sandbox.UI;
using Sandbox.UI.Construct;
using Sandbox;
using EpicDodgeballBattle.Entities.Weapons;

namespace EpicDodgeballBattle.Ui
{
	public class BalloonIndicator : Panel
	{
		public Panel Balloon;

		public BalloonIndicator()
		{
			StyleSheet.Load( "/ui/BalloonIndicator.scss" );

			Balloon = Add.Panel( "balloon" );
		}

        public override void Tick()
        {
            if(Local.Pawn.Inventory.Active is DodgeballWeapon)
				RemoveClass("d-none");
			else
				AddClass("d-none");
        }
	}
}
