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
        int winCounter = 0;
        Label winCount;
        TextBox nameBox;
        string nameSaver;
        CheckBox isActive;
        bool isAc = true;
        ComboBox plType;
        int plT = 0;
        public FlowLayoutPanel GetPanel(int i)
        {
            if (flp == null || flp.IsDisposed)
            {
                color = i;
                flp = new FlowLayoutPanel()
                {
                    AutoSize = true,
                };
                winCount = new Label()
                {
                    Text = winCounter.ToString(),
                    Width = 50
                };
                flp.Controls.Add(winCount);
                isActive = new CheckBox() { Checked = isAc, AutoSize = true };
                flp.Controls.Add(isActive);
                nameBox = new TextBox()
                {
                    Text = $"player-{i}",
                    Width = 50,
                    AutoSize = true
                };
                if (nameSaver != null) nameBox.Text = nameSaver;
                flp.Controls.Add(nameBox);
                plType = new ComboBox()
                {
                    Items = { PlayerType.LocalPlayer, PlayerType.BotPlayer },
                    SelectedIndex = plT
                };
                flp.Controls.Add(plType);
            }
            return flp;
        }
        public void AddScore()
        {
            winCounter++;
            winCount.Text = winCount.ToString();
        }
        public FlowLayoutPanel GetPanel()
        {
            return flp;
        }
        public void Destroy()
        {
            flp.Controls.Remove(winCount);
            flp.Controls.Remove(isActive);
            flp.Controls.Remove(nameBox);
            winCount.Dispose();
            nameBox.Dispose();
            isActive.Dispose();
            flp.Dispose();
        }

        internal Player MakePlayer(MenschADN.screens.GameScreen sc)
        {
            isAc = isActive.Checked;
            nameSaver = nameBox.Text;
            plT = plType.SelectedIndex;
            if (!isActive.Checked)
                return null;
            if (type == PlayerType.LocalPlayer)
                return new LocalPlayer(sc, color, nameSaver);
            else if (type == PlayerType.BotPlayer)
                return new FirstMoveBot(sc, color, nameSaver);
            return null;
        }
        internal Player MakeServerPlayer(MenschADN.screens.GameScreen sc)
        {
            isAc = isActive.Checked;
            nameSaver = nameBox.Text;
            plT = plType.SelectedIndex;
            if (!isActive.Checked)
                return null;
            if (type == PlayerType.LocalPlayer)
                return new ServerPlayer(sc, color, nameSaver);
            else if (type == PlayerType.BotPlayer)
                return new FirstMoveBot(sc, color, nameSaver);
            return null;
        }
    }
}
