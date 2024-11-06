using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenschADN.screens
{
    public abstract class Screen
    {
        internal Displayer parentForm;
        internal Screen oldScreen;
        public Screen(Displayer parent,Screen screenParent)
        {
            parentForm = parent;
            oldScreen = screenParent;
        }

        public abstract void Create();
        public abstract void Destroy();
        public abstract void Resize(object? sender, EventArgs e);
    }
}
