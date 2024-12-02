using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenschADN.screens
{
    public class HelpScreen : screens.Screen
    {
        Button goBack;
        Label functionText;
        Label ruleText;
        public HelpScreen(Displayer parent, Screen screenParent) : base(parent, screenParent)
        { }

        public override void Create()
        {
            goBack = new Button()
            {
                Text="return",
                AutoSize = true,
            };
            goBack.Click += Returning;
            parentForm.Controls.Add(goBack);
            functionText = new Label()
            {
                Text = "Um einen Spieler zu bewegen, musst du auf die zubewegende Figur an clicken.\n",
                AutoSize = true
            };
            parentForm.Controls.Add(functionText);
            ruleText = new Label()
            {
                Text = "Bei einer 6 musst du einen Spielfigur aus den Startfeld bewegen!\n" +
                "Wenn eine 6 gewürfelt wurde, wird dannach nochmal gewürfelt!\n" +
                "Das Startfeld muss freigemacht werden!\n" +
                "Das Zeil ist es alle deiner Figuren in das Haus zu bekommen.",
                AutoSize = true
            };
            parentForm.Controls.Add(ruleText);
        }

        public void Returning(object sender, EventArgs e)
        {
            parentForm.ChangeScreen(oldScreen);
        }

        public override void Destroy()
        {
            parentForm.Controls.Remove(goBack);
            goBack.Dispose();
            parentForm.Controls.Remove(functionText);
            functionText.Dispose();
            parentForm.Controls.Remove(ruleText);
            ruleText.Dispose();
        }

        public override void Resize(object? sender, EventArgs e)
        {
            functionText.Location = new Point((parentForm.Width - functionText.Width) / 2, parentForm.Height / 5);
            ruleText.Location = new Point((parentForm.Width - functionText.Width) / 2, functionText.Location.Y + functionText.Height + goBack.Height / 4);
            goBack.Location = new Point((parentForm.Width - goBack.Width) / 2, ruleText.Location.Y+ ruleText.Height + goBack.Height / 4);
        }
    }
}
