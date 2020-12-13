using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using Train2d.Main.Controls;

namespace Train2d.Main.ViewModel
{
  class LayoutViewModel : ViewModelBase, INotifyPropertyChanged
  {

    public event PropertyChangedEventHandler PropertyChanged;
    protected void NotifyPropertyChanged(string info)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
    }

    private DateTime _time1;
    private DateTime _time2;
    private DispatcherTimer _timer;

    private Random rand;

    public LayoutViewModel()
    {
      int width = 100;
      int height = 100;
      Settings = new LayoutViewSettings { ShowSettings = true, ScaleToPosition = true };
      rand = new Random();
      Positions = new List<Position>();
      for (int i = 0; i < 1000; i++)
      {
        Positions.Add(new Position(rand.NextDouble() * width, rand.NextDouble() * height));
      }
      //TestPosition = Positions[0];

      _time1 = DateTime.UtcNow;
      _time2 = DateTime.UtcNow;

      _timer = new DispatcherTimer();
      _timer.Tick += GameLoop; // set the game timer event called game loop
      _timer.Interval = TimeSpan.FromMilliseconds(20); // this time will tick every 20 milliseconds
      _timer.Start(); // start the timer 

    }

    float elapsedSeconds = 0.0f;
    int numFrames = 0;
    float msToTrigger = 1000.0f;

    private void GameLoop(object sender, EventArgs e)
    {
      _time2 = DateTime.UtcNow;
      float deltaTime = (_time2.Ticks - _time1.Ticks) / 10000000f;
      //Console.WriteLine(deltaTime);  // *float* output {0,2493331}
      //Console.WriteLine(time2.Ticks - time1.Ticks); // *int* output {2493331}
      for (int i = 0; i < Positions.Count; i++)
      {
        var p = Positions[i];
        if (rand.Next(20) == 5)
        {
          p.Movement = (new Vector((rand.NextDouble() * 2) - 1, (rand.NextDouble() * 2) - 1))*50;
        }
        p.Update(deltaTime);
      }
      numFrames++;
      elapsedSeconds += deltaTime;
      if (elapsedSeconds > 1.0f)// msToTrigger)
      {
        FramesPerSecond = numFrames / elapsedSeconds;
        NotifyPropertyChanged(nameof(FramesPerSecond));
        //Console.WriteLine($"FPS: {fps}");


        elapsedSeconds = 0.0f;
        numFrames = 0;
      }

      _time1 = _time2;
    }

    public Position TestPosition { get; set; }

    public List<Position> Positions { get; set; }

    public LayoutViewSettings Settings { get; set; }

    public float FramesPerSecond { get; private set; }

  }

  class Position : INotifyPropertyChanged
  {

    public event PropertyChangedEventHandler PropertyChanged;
    protected void NotifyPropertyChanged(string info)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
    }

    public Position(double x, double y)
    {
      Pos = new Vector(x, y);
      Movement = new Vector();
    }

    public void Update(double delta)
    {
      Pos += Movement * delta;

      NotifyPropertyChanged(nameof(Pos));
    }


    public Vector Movement { get; set; }
    public Vector Pos { get; set; }
  }

}
