﻿using System.Collections.Generic;
using Sandbox;

namespace EpicDodgeballBattle.Systems.Rounds
{
	public abstract partial class BaseRound : BaseNetworkable
	{
		public virtual int RoundDuration => 0;

		public float RoundEndTime { get; set; }

		public List<Player> Players = new();
		
		public RealTimeUntil NextSecondTime { get; private set; }

		public virtual string RoundName => "";

		public virtual bool ShowTimeLeft => false;

		public virtual bool ShowRoundInfo => false;

		public float TimeLeft
		{
			get
			{
				return RoundEndTime - Time.Now;
			}
		}

		[Net]
		public int TimeLeftSeconds { get; set; }

		public void Start()
		{
			if ( Host.IsServer && RoundDuration > 0 )
				RoundEndTime = Time.Now + RoundDuration;

			OnStart();
		}

		public void Finish()
		{
			if ( Host.IsServer )
			{
				RoundEndTime = 0f;
				Players.Clear();
			}

			OnFinish();
		}

		public virtual void OnSecond()
		{
			if ( Host.IsServer )
			{
				if ( RoundEndTime > 0 && Time.Now >= RoundEndTime )
				{
					RoundEndTime = 0f;
					OnTimeUp();
				}
				else
				{
					TimeLeftSeconds = TimeLeft.CeilToInt();
				}
			}
		}
		
		public void AddPlayer( Player player )
		{
			Host.AssertServer();

			if ( !Players.Contains(player) )
				Players.Add( player );
		}
		
		public virtual void OnPlayerKilled( Player player, Entity attacker, DamageInfo damageInfo ) { }

		public virtual void OnPlayerSpawn( Player player ) { }

		public virtual void OnPlayerJoin( Player player ) { }

		public virtual void OnPlayerLeave( Player player )
		{
			Players.Remove( player );
		}

		public virtual void OnTick()
		{
			if ( NextSecondTime )
			{
				OnSecond();
				NextSecondTime = 1f;
			}
		}


		public virtual void OnStart() { }

		protected virtual void OnFinish() { }

		protected virtual void OnTimeUp() { }
	}
}
