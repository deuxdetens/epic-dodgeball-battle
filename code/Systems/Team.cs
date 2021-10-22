using System;
using System.Linq;
using EpicDodgeballBattle.Players;
using Sandbox;

namespace EpicDodgeballBattle.Systems
{
	public enum Team
	{
		None,
		Red,
		Blue
	}

	public static class TeamExtensions
	{
		public static string GetHudClass( this Team team )
		{
			return team switch
			{
				Team.None => "team_none",
				Team.Red => "team_red",
				Team.Blue => "team_blue",
				_ => throw new ArgumentOutOfRangeException( nameof(team), team, null )
			};
		}
		
		public static int GetCount( this Team team )
		{
			return Entity.All.OfType<DodgeballPlayer>().Count( e => e.Team == team );
		}
	}
}
