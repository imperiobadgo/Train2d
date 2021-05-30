using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using Train2d.Main.Controls;

namespace Train2d.Main.ViewModel
{
  public class LayoutViewModel : ViewModelBase
  {

    #region Attributes

    private DateTime _time1;
    private DateTime _time2;
    private DispatcherTimer _timer;

    private Random rand;
    private MainWindowViewModel _parent;

    #endregion


    #region Construct

    public LayoutViewModel(MainWindowViewModel parent)
    {
      _parent = parent;
      LayoutController = new LayoutController();
      Settings = new LayoutViewSettings { ShowSettings = true, ScaleToPosition = true };
      EditorController = new EditorViewModel(this);


      InitializeCommands();

      _time1 = DateTime.UtcNow;
      _time2 = DateTime.UtcNow;

      _timer = new DispatcherTimer();
      _timer.Tick += GameLoop; // set the game timer event called game loop
      _timer.Interval = TimeSpan.FromMilliseconds(20); // this time will tick every 20 milliseconds
      _timer.Start(); // start the timer 

    }

    private void InitializeCommands()
    {
      AddTrackCommand = new DelegateCommand(AddTrackCommandExecute);
    }

    #endregion

    #region Gameloop

    float elapsedSeconds = 0.0f;
    int numFrames = 0;
    float secondsToTrigger = 1.0f;

    private void GameLoop(object sender, EventArgs e)
    {
      _time2 = DateTime.UtcNow;
      float deltaTime = (_time2.Ticks - _time1.Ticks) / 10000000f;
      if (!Pause)
      {
        LayoutController.Update(deltaTime);
        GetCommandController().ExecuteNewCommands();
        //Console.WriteLine($"Updated deltaTime: {deltaTime}");
      }
      numFrames++;
      elapsedSeconds += deltaTime;
      if (elapsedSeconds > secondsToTrigger)
      {
        FramesPerSecond = numFrames / elapsedSeconds;
        NotifyPropertyChanged(nameof(FramesPerSecond));
        //Console.WriteLine($"FPS: {fps}");


        elapsedSeconds = 0.0f;
        numFrames = 0;
      }

      _time1 = _time2;
    }

    #endregion

    #region Methods

    public CommandController GetCommandController()
    {
      return _parent.CommandController;
    }

    #endregion

    #region Commands

    public DelegateCommand AddTrackCommand { get; private set; }

    private void AddTrackCommandExecute(object o)
    {

    }

    #endregion


    #region Properties

    public LayoutViewSettings Settings { get; set; }

    public EditorViewModel EditorController { get; private set; }

    public LayoutController LayoutController { get; private set; }


    public bool Pause { get; set; } = true;

    public float FramesPerSecond { get; private set; }

    #endregion

  }
}
