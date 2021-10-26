using Sandbox;

namespace EpicDodgeballBattle.Players.Loadouts
{
	public class BaseLoadout : BaseNetworkable
	{
		public virtual string Name => "Loadout";

		public virtual string Model => "models/citizen/citizen.vmdl";

		public virtual float Health => 100f;
		
		public virtual void Setup( DodgeballPlayer player )
		{
			player.Inventory.DeleteContents();
			player.SetModel( Model );

			player.Controller = new WalkController();
			player.Animator = new StandardPlayerAnimator();

			player.Camera = new FirstPersonCamera();
			
			player.Health = Health;

			player.RemoveAllDecals();

			player.EnableAllCollisions = true;
			player.EnableDrawing = true;
			player.EnableHideInFirstPerson = true;
			player.EnableShadowInFirstPerson = true;

			player.Clothing.LoadFromClient(player.Client);
			player.Clothing.DressEntity(player);
		}
	}
}
