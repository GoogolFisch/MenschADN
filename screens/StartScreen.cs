using MenschADN.players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenschADN.screens
{
    public class StartScreen : Screen
    {
        Font titleFont;
        Label title;
        Button startGame;
        FlowLayoutPanel playerSelect;
        static PlayerCreator[] plCreate = new PlayerCreator[4];
        public StartScreen(Displayer parent,Screen parentScreen) : base(parent, parentScreen)
        {
        }

        public override void Create()
        {
            titleFont = new Font(FontFamily.GenericSerif, 24, FontStyle.Bold);
            title = new Label
            {
                Font = titleFont,
                AutoSize = true,
                Text = "Mensch Ärger Dich Nicht"
            };
            this.parentForm.Controls.Add(title);
            startGame = new Button()
            {
                AutoSize = true,
                Text = "Start",
                Font = titleFont,
                //Location = new Point(15,15),
            };
            startGame.Click += ChangeToGame;
            parentForm.Controls.Add(startGame);
            playerSelect = new FlowLayoutPanel()
            {
                AutoSize = true,
                FlowDirection = FlowDirection.TopDown
            };
            parentForm.Controls.Add(playerSelect);
            for (int i = 0; i < plCreate.Length; i++)
            {
                plCreate[i] = new PlayerCreator();
                playerSelect.Controls.Add(plCreate[i].GetPanel(i));
            }
            //this.parentForm.ResizeEnd += ;
        }
        private void ChangeToGame(object sender, EventArgs e)
        {
            Player[] plList = new Player[plCreate.Length];
            screens.GameScreen sc = new GameScreen(parentForm, this);
            for (int i = 0; i < plList.Length; i++)
            {
                plList[i] = plCreate[i].MakePlayer(sc);
            }
            sc.GivePlayers(plList);
            parentForm.ChangeScreen(sc);
        }

        public override void Destroy()
        {
            parentForm.Controls.Remove(title);
            title.Dispose();
            parentForm.Controls.Remove(startGame);
            startGame.Dispose();
            parentForm.Controls.Remove(playerSelect);
            playerSelect.Dispose();
            // remove font!
            titleFont.Dispose();
            for (int i = 0; i < plCreate.Length; i++)
            {
                playerSelect.Controls.Remove(plCreate[i].GetPanel());
            }
        }

        public override void Resize(object? sender, EventArgs e)
        {
            title.Location = new Point((parentForm.Width - title.Width) / 2, 15);
            startGame.Location = new Point((parentForm.Width - startGame.Width) / 2, parentForm.Height / 5);
        }
    }
}
