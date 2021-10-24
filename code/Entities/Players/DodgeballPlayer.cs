using EpicDodgeballBattle.Systems;
using Sandbox;

namespace EpicDodgeballBattle.Players
{
	[Library("epic_dodgeball_battle", Title = "Epic Dodgeball Battle")]
	public partial class DodgeballPlayer : Player
	{
		public DodgeballPlayer()
		{
			Inventory = new BaseInventory( this );
			Animator = new StandardPlayerAnimator();
		}

		public override void Respawn()
		{
			base.Respawn();
			
			Rounds.Current?.OnPlayerSpawn( this );
		}

		public void Reset()
		{
			Team = Team.None;
		}

		public override void Simulate( Client client )
		{
			SimulateActiveChild( client, ActiveChild );

			var targetWeapon = Input.ActiveChild as BaseWeapon;
			
			TickPlayerUse();

			var controller = GetActiveController();
			controller?.Simulate( client, this, GetActiveAnimator() );
		}
	}

	public static class DodgeballPlayerExtensions
	{
		public static void BecomeRagdollOnClient(this DodgeballPlayer player, Vector3 velocity, DamageFlags damageFlags, Vector3 forcePos, Vector3 force, int forceBone )
		{
			ModelEntity ent = new()
			{   
				Position = player.Position, 
				Rotation = player.Rotation, 
				Scale = player.Scale, 
				MoveType = MoveType.Physics,
				UsePhysicsCollision = true,
				EnableAllCollisions = true,
				CollisionGroup = CollisionGroup.Debris
			};
			ent.SetModel( player.GetModelName() );
			ent.CopyBonesFrom( player );
			ent.CopyBodyGroups( player );
			ent.CopyMaterialGroup( player );
			ent.TakeDecalsFrom( player );
			ent.EnableHitboxes = true;
			ent.EnableAllCollisions = true;
			ent.SurroundingBoundsMode = SurroundingBoundsType.Physics;
			ent.RenderColor = player.RenderColor;
			ent.PhysicsGroup.Velocity = velocity;

			ent.SetInteractsAs( CollisionLayer.Debris );
			ent.SetInteractsWith( CollisionLayer.WORLD_GEOMETRY );
			ent.SetInteractsExclude( CollisionLayer.Player | CollisionLayer.Debris );
			
			if ( damageFlags.HasFlag( DamageFlags.PhysicsImpact ) )
			{
				PhysicsBody body = forceBone > 0 ? ent.GetBonePhysicsBody( forceBone ) : null;

				if ( body != null )
				{
					body.ApplyImpulseAt( forcePos, force * body.Mass );
				}
				else
				{
					ent.PhysicsGroup.ApplyImpulse( force );
				}
			}

			player.Corpse = ent;
			player.Controller = null;
			player.EnableAllCollisions = false;
			player.EnableDrawing = false;
			
			ent.DeleteAsync( 10.0f );
		}
	}
}
