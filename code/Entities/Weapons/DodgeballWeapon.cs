﻿using System;
using EpicDodgeballBattle.Projectiles;
using Sandbox;

namespace EpicDodgeballBattle.Entities.Weapons
{
	[Library("db_balloon", Title = "Dodgeball Balloon")]
	public class DodgeballWeapon : BaseWeapon
	{
		public Vector3 ShootFrom => Owner.EyePos;
		public Rotation ShootFromAngle => Owner.EyeRot;

		public float IneritVelocity => 1000f;
		
		public float ProjectileForce => 0f;

		public override void AttackPrimary()
		{
			if ( !IsServer )
			{
				return;
			}

			using ( Prediction.Off() )
			{
				FireProjectile();
					
				if ( Owner.Inventory.Drop( this ) )
				{ 
					Delete();	
				}
			}
		}

		private void FireProjectile()
		{
			BalloonProjectile projectile = new();
			projectile.SetModel( "models/ball/ball.vmdl" );
			projectile.RenderColor = Color.Red;
			projectile.Position = ShootFrom + ShootFromAngle.Forward * 50f;
			projectile.Rotation = ShootFromAngle;
			projectile.Initialize();
			projectile.Velocity = ShootFromAngle.Forward * IneritVelocity;
			projectile.PhysicsBody.ApplyForce( ShootFromAngle.Forward * ProjectileForce * 200f );
		}
	}
}
