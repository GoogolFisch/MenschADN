using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenschADN
{
    public abstract class Screen
    {
        Form parentForm;

        public abstract void Create();
        public abstract void Destroy();
    }
}
