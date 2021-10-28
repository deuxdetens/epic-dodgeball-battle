using EpicDodgeballBattle.Players;
using EpicDodgeballBattle.Systems;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace EpicDodgeballBattle.Ui
{
	public class PlayerIndicator : BaseNameTag
	{
		private readonly IconPanel Team;

		public PlayerIndicator(Player player)
			: base(player)
		{
			Team = Add.Icon(null, "team");
		}

		public override void UpdateFromPlayer( Player player )
		{
			var dodgeballPlayer = player as DodgeballPlayer;
			Team.Style.BackgroundColor = dodgeballPlayer.Team.GetRenderColor();
		}
	}
}
