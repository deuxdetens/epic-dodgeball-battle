using EpicDodgeballBattle.Players.Loadouts;
using Sandbox;

namespace EpicDodgeballBattle.Players
{
	public partial class DodgeballPlayer
	{
		[Net] public BaseLoadout Loadout { get; set; }
		public bool IsInLobby => HasLoadout<LobbyLoadout>();
		public bool IsPrisoner => HasLoadout<PrisonerLoadout>();
		public bool IsPlayer => HasLoadout<PlayerLoadout>();
		public bool IsSpectator => HasLoadout<SpectateLoadout>();
		
		public BaseLoadout GiveLoadout<T>() where T : BaseLoadout, new()
		{
			Loadout = new T();
			return Loadout;
		}

		public bool HasLoadout<T>() where T : BaseLoadout
		{
			return Loadout is T;
		}
	}
}
