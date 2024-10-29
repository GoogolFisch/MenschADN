using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenschADN.screens
{
    public abstract class Screen
    {
        internal Form parentForm;
        public Screen(Form parent)
        {
            parentForm = parent;
        }

        public abstract void Create();
        public abstract void Destroy();
        public abstract void Resize(object? sender, EventArgs e);
    }
}
