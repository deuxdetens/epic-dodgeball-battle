using Sandbox;
using Sandbox.UI;

namespace EpicDodgeballBattle.Ui
{
	public class PlayerIndicators : NameTags
	{
        public PlayerIndicators()
			: base()
		{
            StyleSheet.Load("/Ui/PlayerIndicators.scss");
		}

        public override BaseNameTag CreateNameTag( Player player )
		{
			if ( player.Client == null )
				return null;

			return new PlayerIndicator( player )
			{
				Parent = this
			};
		}
    }
}