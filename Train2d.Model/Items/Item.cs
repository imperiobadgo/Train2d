using System;

namespace Train2d.Model.Items
{
  public class Item
  {


    #region Methods

    public void SetGuid(Guid? newId)
    {
      Id = newId;
    }

    public void SetPosition(Coordinate? _position)
    {
      Position = _position;
    }

    #endregion

    #region Properties

    public Guid? Id { get; private set; }
    public Coordinate? Position { get; private set; }

    #endregion

  }
}
