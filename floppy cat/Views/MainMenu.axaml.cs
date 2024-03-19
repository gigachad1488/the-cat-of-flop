using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace floppy_cat.Views;

public partial class MainMenu : Window
{
    public MainMenu()
    {
        InitializeComponent();

        playButton.Click += delegate
        {
            StaticData.speedMult = 1;
            MainWindow mw = new MainWindow();
            StaticData.currentWindow = mw;
            
            mw.Show();
            Hide();
        };

        playHardButton.Click += delegate
        {
            StaticData.speedMult = 2.5f;
            MainWindow mw = new MainWindow();
            StaticData.currentWindow = mw;
            
            mw.Show();
            Hide();
        };

        exitButton.Click += delegate { Close(); };

        StaticData.mainMenu = this;
    }

    public void AddResult(int res)
    {
        resBox.Items.Add(res);
    }
}