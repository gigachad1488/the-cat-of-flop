using Avalonia.Controls;
using Avalonia.Input;

namespace floppy_cat.Views;

public partial class MainWindow : Window
{
    private int count;

    public int Count
    {
        get
        {
            return count;
        }
        set
        {
            count = value;
            countText.Text = count.ToString();
        }
    }

    public static MainWindow instance;

    private Floppy floppy;
    private ObstaclesSpawner spawner;
    public MainWindow()
    {
        InitializeComponent();

        UpdateHandler.Init();

        instance = this;

        Canvas.Children.Clear();

        StaticData.canvas = Canvas;

        spawner = new ObstaclesSpawner();
        spawner.Init(Canvas);

        floppy = new Floppy();
        floppy.Init(Canvas, spawner);

        flopname.Text = floppy.name;

        Count = 0;
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {
        base.OnKeyUp(e);
        if (e.Key == Key.Space)
        {
            floppy.Jump();
        }
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);

        floppy.Jump();
    }
}