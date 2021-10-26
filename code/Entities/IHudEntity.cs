using EpicDodgeballBattle.Ui;

namespace EpicDodgeballBattle.Entities
{
	public interface IHudEntity
	{
		Vector3 LocalCenter { get; }
		EntityHud Hud { get; set; }
	}
}
