using Sandbox.UI;
using Sandbox;
using EpicDodgeballBattle.Entities.Weapons;
using Sandbox.UI.Construct;
using System;

namespace EpicDodgeballBattle.Ui
{
	public class BalloonIndicator : Panel
	{
		private readonly Panel Balloon;
		private readonly Label Force;
		
		private DodgeballWeapon DodgeballWeapon { get; set; }

		public BalloonIndicator()
		{
			StyleSheet.Load( "/Ui/BalloonIndicator.scss" );

			Balloon = Add.Panel( "balloon" );
			Force = Balloon.Add.Label(null, "force");
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

			if(DodgeballWeapon != null)
				Force.SetText($"{Math.Round(DodgeballWeapon.ProjectileForce)} %");

			base.Tick();
        }
	}
}
