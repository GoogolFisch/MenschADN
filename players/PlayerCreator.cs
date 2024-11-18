using MenschADN.screens;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenschADN.players
{
    public class PlayerCreator
    {
        public string name;
        public int color;
        public PlayerType type{ get { return (PlayerType)plType.SelectedItem; } }

        FlowLayoutPanel flp;
        TextBox nameBox;
        CheckBox isActive;
        ComboBox plType;
        public FlowLayoutPanel GetPanel(int i)
        {
            if (flp == null)
            {
                color = i;
                flp = new FlowLayoutPanel()
                {
                    AutoSize = true,
                };
                isActive = new CheckBox() { Checked = true, AutoSize = true };
                flp.Controls.Add(isActive);
                nameBox = new TextBox()
                {
                    Text = $"player-{i}",
                    Width = 50,
                    AutoSize = true
                };
                flp.Controls.Add(nameBox);
                plType = new ComboBox()
                {
                    Items = { PlayerType.LocalPlayer, PlayerType.BotPlayer },
                    SelectedIndex = 0
                };
                flp.Controls.Add(plType);
            }
            return flp;
        }
        public FlowLayoutPanel GetPanel()
        {
            return flp;
        }
        public void Destroy()
        {
            flp.Controls.Remove(isActive);
            flp.Controls.Remove(nameBox);
            nameBox.Dispose();
            isActive.Dispose();
            flp.Dispose();
        }

        internal Player MakePlayer(MenschADN.screens.GameScreen sc)
        {
            if(!isActive.Checked)
                return null;
            if(type == PlayerType.LocalPlayer)
                return new LocalPlayer(sc, color);
            else if(type == PlayerType.BotPlayer)
                return new FirstMoveBot(sc,color);
            return null;
        }
    }
}
