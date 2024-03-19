using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Media.Transformation;
using Avalonia.Styling;

namespace floppy_cat.Views;

public class Floppy
{
    private float g = 3f;
    public Size size = new Size(20, 20);

    private double currentPositionY;
    

    public Rectangle collider;

    private Canvas canvas;
    private ObstaclesSpawner spawner;

    public string name;


    private float jumpForce = 13f;
    private float currentJumpForce = 0;
    private float jumpTimer = 0;
    
    private double currentRotation = 0;

    private bool jumping;
    
    public void Init(Canvas canvas, ObstaclesSpawner spawner)
    {
        this.canvas = canvas;
        this.spawner = spawner;

        Random r = new Random();
        name = "flop " + r.Next(0, 1000);

        collider = new Rectangle();
        collider.Classes.Add("f");
        collider.Width = size.Width;
        collider.Height = size.Height;
        collider.RadiusX = size.Width / 2;
        collider.RadiusY = size.Height / 2;
        collider.Classes.Add("flop");
        currentPositionY = 100;
        Canvas.SetBottom(collider, currentPositionY);
        Canvas.SetLeft(collider, 200);
        canvas.Children.Add(collider);       

        UpdateHandler.updateEvent += Update;
        StaticData.floppy = this;
    }
    
    public void Update()
    {
        currentPositionY -= g;     

        if (jumping)
        {
            currentPositionY += currentJumpForce;
            currentJumpForce *= 0.90f;
            jumpTimer--;
            Random random = new Random();
            currentRotation = Double.Lerp(currentRotation, -40, UpdateHandler.frameTime / 30f);
            
            Style buttonStyle = new Style(x => x.OfType<Rectangle>())
            {
                Setters =
                {
                    new Setter
                    {
                        Property = Rectangle.RenderTransformProperty,
                        Value = TransformOperations.Parse($"rotate({Double.Clamp(currentRotation, -40, 40)}deg)")
                    }
                }
            };
        
            collider.Styles.Add(buttonStyle);
            
            if (jumpTimer < 0f)
            {
                jumping = false;
            }         
        }
        else
        {
            currentRotation = Double.Lerp(currentRotation, 40, UpdateHandler.frameTime / 80f);

            Style buttonStyle = new Style(x => x.OfType<Rectangle>())
            {
                Setters =
                {
                    new Setter
                    {
                        Property = Rectangle.RenderTransformProperty,
                        Value = TransformOperations.Parse($"rotate({Double.Clamp(currentRotation, -40, 40)}deg)")

                    }
                }
            };

            collider.Styles.Add(buttonStyle);
        }

        Canvas.SetBottom(collider, currentPositionY);

        if (currentPositionY < -2|| currentPositionY > canvas.Bounds.Height - 40)
        {
            StaticData.floppy.collider.Fill = new SolidColorBrush(Colors.Red);
            UpdateHandler.updateEvent -= Update;
            UpdateHandler.timeScale = 0;
            UpdateHandler.end = true;
            EndScreen es = new EndScreen(MainWindow.instance.Count, spawner, this);
            es.Show();

            Random r = new Random();
        }
    }

    public void Jump()
    {
        jumping = true;
        jumpTimer = 15f;
        currentJumpForce = jumpForce;
    }

    public void Delete()
    {
        canvas.Children.Remove(collider);
    }
}