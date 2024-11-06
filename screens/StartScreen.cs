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
            //this.parentForm.ResizeEnd += ;
        }
        private void ChangeToGame(object sender, EventArgs e)
        {
            parentForm.ChangeScreen(new GameScreen(parentForm,this));
        }

        public override void Destroy()
        {
            parentForm.Controls.Remove(title);
            title.Dispose();
            parentForm.Controls.Remove(startGame);
            startGame.Dispose();
            // remove font!
            titleFont.Dispose();
        }

        public override void Resize(object? sender, EventArgs e)
        {
            title.Location = new Point((parentForm.Width - title.Width) / 2, 15);
            startGame.Location = new Point((parentForm.Width - startGame.Width) / 2, parentForm.Height / 5);
        }
    }
}
