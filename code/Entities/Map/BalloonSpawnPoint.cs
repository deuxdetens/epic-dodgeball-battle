using EpicDodgeballBattle.Entities.Projectiles;
using Hammer;
using Sandbox;

namespace EpicDodgeballBattle.Entities.Map
{
	[Library("balloon_spawn", Description = "The balloon spawn point")]
	[EntityTool("Balloon spawn", "Dodgeball", "Defines a point where the balloon can (re)spawn")]
	[EditorModel("models/ball/ball.vmdl")]
	public class BalloonSpawnPoint : Prop
	{
		public override void Spawn()
		{
			_ = new BalloonProjectile
			{
				Transform = Transform,
				RenderColor = RenderColor
			};
		}
    }
}