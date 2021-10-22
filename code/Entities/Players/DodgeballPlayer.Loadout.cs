using EpicDodgeballBattle.Players.Loadouts;
using Sandbox;

namespace EpicDodgeballBattle.Players
{
	public partial class DodgeballPlayer
	{
		[Net] public BaseLoadout Loadout { get; set; }
		
		public BaseLoadout GiveLoadout<T>() where T : BaseLoadout, new()
		{
			Loadout = new T();
			return Loadout;
		}
	}
}
