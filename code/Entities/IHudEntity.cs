using EpicDodgeballBattle.Ui;
using Sandbox.Internal;

namespace EpicDodgeballBattle.Entities
{
	public interface IHudEntity
	{
		Vector3 LocalCenter { get; }
		EntityHud Hud { get; set; }
	}
}
