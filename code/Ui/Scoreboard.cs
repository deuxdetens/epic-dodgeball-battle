using Sandbox.UI;
using Sandbox;
using Sandbox.UI.Construct;
using System.Collections.Generic;
using System.Linq;

namespace EpicDodgeballBattle.Ui
{
	public class Scoreboard : Panel
	{
        private readonly Panel Header;
        private readonly Panel Canvas;
        private readonly Dictionary<Client, Panel> Rows;

		public Scoreboard()
		{
		    StyleSheet.Load( "/ui/Scoreboard.scss" );

            Header = Add.Panel("header");
            Header.Add.Label("Avatar", "avatar");
            Header.Add.Label("Name", "name");
            Header.Add.Label("Team", "team");
            Header.Add.Label("Score", "score");

            Canvas = Add.Panel("canvas");

            Rows = new Dictionary<Client, Panel>();
		}

        private Panel CreateEntry(Client client)
        {
            var entry = Canvas.AddChild<ScoreboardEntry>();
            entry.Client = client;

            Rows[client] = entry;

            return entry;
        }

		public override void Tick()
		{
            SetClass("d-none", !Input.Down(InputButton.Score));

            if(IsVisible)
            {
                foreach(var client in Client.All.Except(Rows.Keys))
                    CreateEntry(client);

                foreach(var client in Rows.Keys.Except(Client.All))
                    if(Rows.TryGetValue(client, out var row))
                    {
                        row.Delete();
                        Rows.Remove(client);
                    }
            }

			base.Tick();
		}


	}
}