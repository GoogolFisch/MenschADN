using MenschADN.assets;
using MenschADN.screens;

namespace MenschADN
{
    public partial class Displayer : Form
    {
        screens.Screen currentScreen;
        public Displayer()
        {
            InitializeComponent();
            ImageLoader imgLoad = new();
            currentScreen = new StartScreen(this,null);
            this.Resize += ResizeHandler;
            this.ResizeEnd += ResizeHandler;
            currentScreen.Create();
        }

        private void ResizeHandler(object? sender, EventArgs e)
        {
            if (currentScreen != null) {
                currentScreen.Resize(sender, e);
            }
        }
        public void ChangeScreen(screens.Screen newScreen)
        {
            currentScreen.Destroy();
            currentScreen = newScreen;
            newScreen.Create();
            newScreen.Resize(this,EventArgs.Empty);
        }
    }
}
