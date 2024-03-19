using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;

namespace floppy_cat.Views;

public class Obstacle
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
            StaticData.floppy.collider.Fill = new SolidColorBrush(Colors.Red);         
            UpdateHandler.timeScale = 0;
            UpdateHandler.end = true;
            EndScreen es = new EndScreen(MainWindow.instance.Count, spawner);
            es.Show();            
        }
    }

    public void Delete()
    {
        UpdateHandler.updateEvent -= Update;
        spawner.canvas.Children.Remove(transform);
        spawner.obstacles.Remove(this);
    }
}