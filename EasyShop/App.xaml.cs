namespace EasyShop
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = new Window(new MainPage())
            {
                Title = "EasyShop",
                MinimumWidth = 400,
                MinimumHeight = 800,
                Width = 400,
                Height = 800
            };

            return window;
        }
    }
}
