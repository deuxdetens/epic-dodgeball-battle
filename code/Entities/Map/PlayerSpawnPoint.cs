using EpicDodgeballBattle.Systems;
using Hammer;
using Sandbox;

namespace EpicDodgeballBattle.Entities.Map
{
    [Library("player_spawn_point", Description = "The player spawn point")]
    [EntityTool("Player spawn", "Dodgeball", "Defines a point where the player can (re)spawn")]
    [EditorModel("models/citizen/citizen.vmdl")]
	public class PlayerSpawnPoint : Entity
	{
        [Property]
        public Team Team { get; set; }
        [Property]
        public bool IsJail { get; set; }
    }
}