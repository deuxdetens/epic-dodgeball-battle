using Sandbox;

namespace EpicDodgeballBattle.Players
{
	public partial class DodgeballPlayer
	{
        [ClientRpc]
        private void BecomeRagdollOnClient( Vector3 velocity, DamageFlags damageFlags, Vector3 forcePos, Vector3 force, int forceBone )
		{
			var ent = new ModelEntity()
			{   
				Position = Position, 
				Rotation = Rotation, 
				Scale = Scale, 
				MoveType = MoveType.Physics,
				UsePhysicsCollision = true,
				EnableAllCollisions = true,
				CollisionGroup = CollisionGroup.Debris
			};
			ent.SetModel( GetModelName() );
			ent.CopyBonesFrom( this );
			ent.CopyBodyGroups( this );
			ent.CopyMaterialGroup( this );
			ent.TakeDecalsFrom( this );
			ent.EnableHitboxes = true;
			ent.EnableAllCollisions = true;
			ent.SurroundingBoundsMode = SurroundingBoundsType.Physics;
			ent.RenderColor = RenderColor;
			ent.PhysicsGroup.Velocity = velocity;

			ent.SetInteractsAs( CollisionLayer.Debris );
			ent.SetInteractsWith( CollisionLayer.WORLD_GEOMETRY );
			ent.SetInteractsExclude( CollisionLayer.Player | CollisionLayer.Debris );

            foreach(var child in Children)
            {
                if(!child.Tags.Has("clothes"))
                    continue;

                if(child is not ModelEntity e)
                    continue;

                var clothing = new ModelEntity()
                {
                    RenderColor = e.RenderColor
                };
                clothing.SetModel(e.GetModelName());
                clothing.SetParent(ent, true);
                clothing.CopyBodyGroups(e);
                clothing.CopyMaterialGroup(e);
            }
			
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

			Corpse = ent;
			
			ent.DeleteAsync( 10.0f );
		}
    }
}