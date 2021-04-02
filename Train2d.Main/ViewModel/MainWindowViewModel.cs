using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Train2d.Main.Controls;
using Train2d.Model;

namespace Train2d.Main.ViewModel
{
  public class MainWindowViewModel : ViewModelBase
  {

    #region Construct
    public MainWindowViewModel()
    {
      CommandController = new CommandController();
      UserSettings = new UserSettings();
      InitializeCommands();
      Layouts = new List<LayoutViewModel>();
      Layouts.Add(new LayoutViewModel(this));
      SelectedView = Layouts[0];
    }

    private void InitializeCommands()
    {
      SaveLayoutCommand = new DelegateCommand(SaveLayoutCommandExecute);
      LoadLayoutCommand = new DelegateCommand(LoadLayoutCommandExecute);
    }

    #endregion

    #region Methods



    #endregion

    #region Commands

    public DelegateCommand SaveLayoutCommand { get; private set; }

    private void SaveLayoutCommandExecute(object o)
    {
      SaveFileDialog sfd = new SaveFileDialog()
      {
        //InitialDirectory = Application.StartupPath + "\\Scripts\\",
        Title = "Save Layout",
        CheckPathExists = true,
        DefaultExt = "MyLayout",
        Filter = "Layout (*.layout)|*.layout|All files (*.*)|*.*",
        FilterIndex = 1,
        RestoreDirectory = true
      };

      if (sfd.ShowDialog() == true)
      {
        var serializer = Serialization.SerializerContainer.GetExtendedSerializer();
        FileStream stream = new FileStream(sfd.FileName, FileMode.OpenOrCreate);
        using (XmlWriter writer = XmlWriter.Create(stream))
        {
          Layout layoutToSave = SelectedView.LayoutController.GetLayout();
          serializer.Serialize(writer, layoutToSave);
          writer.Flush();
        }
      }
    }

    public DelegateCommand LoadLayoutCommand { get; private set; }

    private void LoadLayoutCommandExecute(object o)
    {
      OpenFileDialog ofd = new OpenFileDialog()
      {
        Title = "Load Layout",
        CheckPathExists = true,
        DefaultExt = "MyLayout",
        Filter = "Layout (*.layout)|*.layout|All files (*.*)|*.*",
        FilterIndex = 1,
        RestoreDirectory = true
      };
      if (ofd.ShowDialog() == true)
      {
        var serializer = Serialization.SerializerContainer.GetExtendedSerializer();
        try
        {
          FileStream stream = new FileStream(ofd.FileName, FileMode.OpenOrCreate);
          using (XmlReader reader = XmlReader.Create(stream))
          {
            Layout loadedLayout = (Layout)serializer.Deserialize(reader);
            SelectedView.LayoutController.SetLayout(loadedLayout);
          }
        }
        catch(Exception ex)
        {

        }

      }
    }

    #endregion

    #region Properties

    public List<LayoutViewModel> Layouts;

    public LayoutViewModel SelectedView { get; set; }

    public UserSettings UserSettings { get; set; }

    public CommandController CommandController { get; private set; }

    #endregion

  }


}
