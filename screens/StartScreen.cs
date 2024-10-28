using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenschADN.screens
{
    public class StartScreen : Screen
    {
        Label title;
        public StartScreen(Form parent) : base(parent)
        {
        }

        public override void Create()
        {
            title = new Label();
            title.Text = "Mensch Ärger Dich Nicht";
            this.parentForm.Controls.Add(title);
            //this.parentForm.ResizeEnd += ;
        }

        public override void Destroy()
        {
            parentForm.Controls.Remove(title);
        }

        public override void Resize(object? sender, EventArgs e)
        {
        }
    }
}
