using Train2d.Main.Commands;
using Train2d.Model.Converter;

namespace Train2d.Main.ViewModel
{
  public class EditorViewModel : ViewModelBase
  {
    #region Attributes

    private readonly LayoutViewModel _parent;    

    #endregion

    #region Construct

    public EditorViewModel(LayoutViewModel parent)
    {
      _parent = parent;
      _parent.Settings.SetOnMouseLeftButtonDownAction(OnMouseLeftButtonUp);
    }

    #endregion

    #region Methods

    private void OnMouseLeftButtonUp()
    {
      if (PlaceTracks)
      {
        BaseItemViewModel newItem = new BaseItemViewModel();
        CreateItemCommand createCommand = new CreateItemCommand(_parent, newItem);
        _parent.GetCommandController().AddCommandAndExecute(createCommand);
        PositionItemCommand positionItem = new PositionItemCommand(_parent, newItem, _parent.Settings.MousePosition.ToCoordinate());
        _parent.GetCommandController().AddCommandAndExecute(positionItem);
      }
    }

    #endregion

    #region Properties

    public bool PlaceTracks { get; set; }

    #endregion

  }
}
