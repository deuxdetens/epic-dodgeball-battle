using EpicDodgeballBattle.Players;
using EpicDodgeballBattle.Systems;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace EpicDodgeballBattle.Ui
{
	public class PlayerIndicator : Panel
	{
		private readonly Panel Container;
        private readonly Image Avatar;
        private readonly Label Name;
		private readonly IconPanel Team;

		private Client Client { get; set; }

		public PlayerIndicator()
		{
			StyleSheet.Load("/Ui/PlayerIndicator.scss");

			Container = Add.Panel("container");
            Avatar = Container.Add.Image("avatar:0", "avatar");
			Name = Container.Add.Label("Name", "name");
			Team = Container.Add.Icon(null, "team");
		}

		[Event.Tick.Client]
		private void Tick()
		{
			if(Parent is EntityHud hud
				&& hud.Entity is DodgeballPlayer player)
			{
				Team.Style.BackgroundColor = player.Team.GetRenderColor();

				if(player.Client != null
					&& Client != player.Client)
				{
					Client = player.Client;

					Avatar.SetTexture($"avatar:{Client.SteamId}");
            		Name.SetText(Client.Name);
				}
			}
		}
	}
}
