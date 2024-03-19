using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using System.Diagnostics;

namespace floppy_cat.Views;

public class Counter
{
    private float speed = 1.5f;
    
    private Rectangle transform;
    private ObstaclesSpawner spawner;
    
    public void Init(Rectangle transform, ObstaclesSpawner spawner)
    {
        this.transform = transform;
        this.spawner = spawner;
        speed *= StaticData.speedMult;
        UpdateHandler.updateEvent += Update;
    }
    
    public void Update()
    {
        Canvas.SetRight(transform, Canvas.GetRight(transform) + speed);

        if (Canvas.GetRight(transform) > spawner.canvas.Bounds.Width)
        {
            Delete();
        }

        if (transform.Bounds.Intersects(StaticData.floppy.collider.Bounds))
        {
            MainWindow.instance.Count++;
            Delete();
        }
    }

    public void Delete()
    {
        UpdateHandler.updateEvent -= Update;
        spawner.canvas.Children.Remove(transform);
        spawner.counters.Remove(this);

        Debug.WriteLine("DELETED COUNTER");
    }
}