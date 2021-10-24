﻿using EpicDodgeballBattle.Systems;
using Sandbox;

namespace EpicDodgeballBattle.Players
{
	public partial class DodgeballPlayer
	{
		[Net, Change] public Team Team { get; private set; }
		
		public void SetTeam( Team team )
		{
			Team = team;
			Client.SetInt( "team", (int)team );
		}
		
		protected virtual void OnTeamChanged( Team oldTeam, Team newTeam )
		{
			if ( IsLocalPawn )
			{
				Local.Hud.RemoveClass( oldTeam.GetHudClass() );
				Local.Hud.AddClass( newTeam.GetHudClass() );
			}
		}
	}
}
