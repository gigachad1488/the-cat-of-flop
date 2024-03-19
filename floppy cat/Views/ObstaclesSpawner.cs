
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Avalonia.Animation.Easings;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Threading;
using Avalonia.Styling;
using Avalonia.Media.Transformation;

namespace floppy_cat.Views;

public class ObstaclesSpawner
{
    public float spawnTime = 100f;
    public float spawnTimer = 0f;

    public Canvas canvas;

    public float rightSpawnOffset = -45f;

    private int count = 0;

    private Color color = Colors.Aqua;
    private Color bgColor = Colors.Black;

    public List<Obstacle> obstacles = new List<Obstacle>();
    public List<Counter> counters = new List<Counter>();
    public void Init(Canvas canvas)
    {
        this.canvas = canvas;
        bgColor = Colors.White;
        spawnTime /= (1 + (StaticData.speedMult - 1));
        UpdateHandler.updateEvent += Update;

    }
    public async void Spawn()
    {
        if (count >= 8)
        {
            Random r = new Random();

            count = 0;
            color = new Color(255, Convert.ToByte(r.Next(0, 255)), Convert.ToByte(r.Next(0, 255)), Convert.ToByte(r.Next(0, 255)));
            StartAnim(bgColor);
        }

        Random random = new Random();
        float rh = random.Next(-65, 65);
        float dif = Math.Abs(rh * (StaticData.speedMult - 1));

        Rectangle rt = new Rectangle();
        rt.Fill = new SolidColorBrush(color);
        rt.Height = 120 + rh + (dif * 0.2f);
        rt.Width = 30;
        Canvas.SetTop(rt, 0);
        Canvas.SetRight(rt, rightSpawnOffset);
        Obstacle obt = new Obstacle();
        obt.Init(rt, this);

        Rectangle rb = new Rectangle();
        rb.Fill = new SolidColorBrush(color);
        rb.Height = 120 - rh + (dif * 0.2f);
        rb.Width = 30;
        Canvas.SetBottom(rb, 0);
        Canvas.SetRight(rb, rightSpawnOffset);
        Obstacle obb = new Obstacle();
        obb.Init(rb, this);

        Rectangle c = new Rectangle();
        c.Height = canvas.Bounds.Height;
        c.Width = 15;
        Canvas.SetBottom(c, 0);
        Canvas.SetRight(c, rightSpawnOffset);
        Counter cc = new Counter();
        cc.Init(c, this);

        canvas.Children.Add(rt);
        canvas.Children.Add(rb);
        canvas.Children.Add(c);

        obstacles.Add(obt);
        obstacles.Add(obb);
        counters.Add(cc);

        spawnTimer = spawnTime;

        count++;
    }

    public void StartAnim(Color curColor)
    {
        Random rand = new Random();
        Animation anim = new Animation();
        anim.FillMode = FillMode.Forward;
        anim.IterationCount = new IterationCount(1);
        anim.Duration = TimeSpan.FromSeconds(8 / (1 + (StaticData.speedMult - 1)));
        bgColor = new Color(0, Convert.ToByte(rand.Next(0, 255)), Convert.ToByte(rand.Next(0, 255)), Convert.ToByte(rand.Next(0, 255)));
        LinearGradientBrush l1 = new LinearGradientBrush();
        l1.StartPoint = new Avalonia.RelativePoint(0, 1, Avalonia.RelativeUnit.Relative);
        l1.EndPoint = new Avalonia.RelativePoint(1, 1, Avalonia.RelativeUnit.Relative);
        l1.GradientStops.Add(new GradientStop
        {
            Offset = 0,
            Color = curColor,
        });
        l1.GradientStops.Add(new GradientStop
        {
            Offset = 0,
            Color = Colors.White,
        });
        l1.GradientStops.Add(new GradientStop
        {
            Offset = 1,
            Color = bgColor
        });
        KeyFrame keyFrame1 = new KeyFrame
        {
            Cue = new Cue(1),
            Setters =
            {
                new Setter
                {
                    Property = Canvas.BackgroundProperty,
                    Value = l1
                }
            }
        };

        LinearGradientBrush l2 = new LinearGradientBrush();
        l2.StartPoint = new Avalonia.RelativePoint(0, 1, Avalonia.RelativeUnit.Relative);
        l2.EndPoint = new Avalonia.RelativePoint(1, 1, Avalonia.RelativeUnit.Relative);
        l2.GradientStops.Add(new GradientStop
        {
            Offset = 0,
            Color = curColor,
        });
        l2.GradientStops.Add(new GradientStop
        {
            Offset = 1,
            Color = Colors.White,
        });
        l2.GradientStops.Add(new GradientStop
        {
            Offset = 1,
            Color = bgColor
        });
        KeyFrame keyFrame2 = new KeyFrame
        {
            Cue = new Cue(0),
            Setters =
            {
                new Setter
                {
                    Property = Canvas.BackgroundProperty,
                    Value = l2
                }
            }
        };

        
        anim.Children.Add(keyFrame1);
        anim.Children.Add(keyFrame2);

        Style style = new Style(x => x.OfType<Canvas>())
        {
            Animations =
            {
                anim
            }
        };
        
        /*
        Transitions tr = new Transitions
        {
        new BrushTransition
        {
            Property = Canvas.BackgroundProperty,
            Duration = TimeSpan.FromSeconds(4),
        }
        };
        StaticData.canvas.Transitions = tr;

        StaticData.canvas.Background = l1;
        */
        
        StaticData.canvas.Styles.Clear();
        StaticData.canvas.Styles.Add(style);
    }

    private void Update()
    {
        spawnTimer--;
        if (spawnTimer <= 0)
        {
            Spawn();
        }
    }

    public void DeleteAll()
    {
        for (int i = 0; i < obstacles.Count; i++)
        {
            obstacles[i].Delete();
            obstacles[i] = null;
        }

        for (int i = 0; i < counters.Count; i++)
        {
            counters[i].Delete();
        }

        obstacles.Clear();
        counters.Clear();

    }
}