using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace floppy_cat.Views;

public partial class EndScreen : Window
{
    public EndScreen()
    {
        InitializeComponent();
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        
    }

    public EndScreen(int res, ObstaclesSpawner spawner)
    {
        InitializeComponent();

        resText.Text = res.ToString();

        okButton.Click += delegate
        {
            if (StaticData.currentWindow != null)
            {
                StaticData.currentWindow.Close();
                StaticData.currentWindow = null;
            }
            StaticData.mainMenu.Show();
            StaticData.mainMenu.AddResult(res);
            spawner.DeleteAll();
            StaticData.floppy.Delete();
            StaticData.floppy = null;
            spawner = null;
            Close();
        };
    }

    public EndScreen(int res, ObstaclesSpawner spawner, Floppy flop)
    {
        InitializeComponent();

        resText.Text = res.ToString();

        okButton.Click += delegate
        {
            if (StaticData.currentWindow != null) 
            {
                StaticData.currentWindow.Close();
                StaticData.currentWindow = null;
            }
            StaticData.mainMenu.Show();
            spawner.DeleteAll();
            StaticData.floppy.Delete();
            StaticData.floppy = null;
            spawner = null;
            Close();
        };
    }
}