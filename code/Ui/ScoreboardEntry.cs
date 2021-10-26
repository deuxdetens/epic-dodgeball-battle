using Sandbox.UI;
using Sandbox;
using Sandbox.UI.Construct;
using EpicDodgeballBattle.Players;
using EpicDodgeballBattle.Systems;

namespace EpicDodgeballBattle.Ui
{
	public class ScoreboardEntry : Panel
	{
		public Client Client { get; set; }
		public DodgeballPlayer Player => Client.Pawn as DodgeballPlayer;

		private readonly Image Avatar;
		private readonly Label Name;
		private readonly Label Team;
		private readonly Label Score;

		public ScoreboardEntry()
		{
			Avatar = Add.Image(null, "avatar");
			Name = Add.Label(null, "name");
			Team = Add.Label(null, "team");
			Score = Add.Label(null, "score");
		}

		public override void Tick()
		{
			if(IsVisible && Client.IsValid())
				UpdateData();

			base.Tick();
		}

		private void UpdateData()
		{
			Avatar.SetTexture($"avatar:{Client.SteamId}");
			Name.SetText(Client.Name);
			Team.SetText(Player.Team.ToString());
			Score.SetText(Client.GetInt("score").ToString());

			Name.Style.FontColor = Player.Team.GetRenderColor();
			Team.Style.FontColor = Player.Team.GetRenderColor();
			Score.Style.FontColor = Player.Team.GetRenderColor();
		}
	}
}