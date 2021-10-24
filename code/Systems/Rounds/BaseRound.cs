using System.Collections.Generic;
using EpicDodgeballBattle.Players;
using Sandbox;

namespace EpicDodgeballBattle.Systems
{
	public abstract partial class BaseRound : BaseNetworkable
	{
		public virtual int RoundDuration => 0;
		public bool IsFinish { get; set; }
		
		public float RoundEndTime { get; set; }

		public List<DodgeballPlayer> Players = new();
		
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
			if ( IsFinish )
			{
				return;
			}
			
			if ( Host.IsServer )
			{
				RoundEndTime = 0f;
				Players.Clear();
			}

			IsFinish = true;
			OnFinish();
		}

		protected virtual void OnSecond()
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
		
		public void AddPlayer( DodgeballPlayer player )
		{
			Host.AssertServer();

			if ( !Players.Contains(player) )
				Players.Add( player );
		}
		
		public virtual void OnPlayerIsPrisoner( DodgeballPlayer player, DodgeballPlayer attacker ) { }

		public virtual void OnPlayerSpawn( DodgeballPlayer player ) { }

		public virtual void OnPlayerJoin( DodgeballPlayer player ) { }

		public virtual void OnPlayerLeave( DodgeballPlayer player )
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


		protected virtual void OnStart() { }

		protected virtual void OnFinish() { }

		protected virtual void OnTimeUp() { }
	}
}
