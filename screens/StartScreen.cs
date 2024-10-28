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
        public StartScreen(Form parent) : base(parent)
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
            //this.parentForm.ResizeEnd += ;
        }

        public override void Destroy()
        {
            parentForm.Controls.Remove(title);
            title.Dispose();
            titleFont.Dispose();
        }

        public override void Resize(object? sender, EventArgs e)
        {
            title.Location = new Point((parentForm.Width - title.Width) / 2, 15);
        }
    }
}
