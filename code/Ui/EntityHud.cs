using EpicDodgeballBattle.Entities;
using Sandbox;
using Sandbox.UI;

namespace EpicDodgeballBattle.Ui
{
	public class EntityHud : WorldPanel
	{
        public IHudEntity Entity { get; set; }
        public float UpOffset { get; set; } = 80f;
        public float MaxDistanceView { get; set; } = -1f;

        public EntityHud()
        {
            StyleSheet.Load("/Ui/EntityHud.scss");

            SceneObject.ZBufferMode = ZBufferMode.None;

            PanelBounds = new Rect(-1000, -1000, 2000, 2000);
        }

		public override void Tick()
		{
            if((Entity is Entity entity) && entity.IsValid())
            {
                var cameraPosition = CurrentView.Position;

                Position = entity.Position + Entity.LocalCenter + (Vector3.Up * UpOffset);
                Rotation = Rotation.LookAt(cameraPosition - Position);

                var distanceToCamera = Position.Distance(cameraPosition);
                Scale = distanceToCamera.Remap(0f, 2000f, 2f, 40f);

                SetClass("d-none", MaxDistanceView > 0 && distanceToCamera > MaxDistanceView);
            }

			base.Tick();
		}
	}
}
