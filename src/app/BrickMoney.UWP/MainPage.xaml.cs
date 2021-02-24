namespace BrickMoney.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new BrickMoney.App(new UwpInitializer()));
        }
    }
}
