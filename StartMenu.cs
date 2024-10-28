using MenschADN.screens;

namespace MenschADN
{
    public partial class StartMenu : Form
    {
        Screen currentScreen;
        public StartMenu()
        {
            InitializeComponent();
            currentScreen = new StartScreen(this);
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
        public void ChangeScreen(Screen newScreen)
        {
            currentScreen.Destroy();
            currentScreen = newScreen;
            newScreen.Create();
        }
    }
}
