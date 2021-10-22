using Sandbox.UI;
using Sandbox.UI.Construct;
using Sandbox;
using EpicDodgeballBattle.Entities.Weapons;

namespace EpicDodgeballBattle.Ui
{
	public class BalloonIndicator : Panel
	{
		public Panel Balloon;
		
		private BaseWeapon DodgeballWeapon = null;

		public BalloonIndicator()
		{
			StyleSheet.Load( "/ui/BalloonIndicator.scss" );

			Balloon = Add.Panel( "balloon" );
		}

		private void OnWeaponChange()
		{
			if(DodgeballWeapon != null)
			{
				Balloon.Style.BackgroundColor = DodgeballWeapon.RenderColor;
				RemoveClass("d-none");
			}
			else
				AddClass("d-none");
		}

        public override void Tick()
        {
			if(Local.Pawn.Inventory.Active == null && DodgeballWeapon != null)
			{
				DodgeballWeapon = null;
				OnWeaponChange();
			}
			else if(Local.Pawn.Inventory.Active is DodgeballWeapon && DodgeballWeapon == null)
			{
				DodgeballWeapon = Local.Pawn.Inventory.Active as DodgeballWeapon;
				OnWeaponChange();
			}
        }
	}
}
