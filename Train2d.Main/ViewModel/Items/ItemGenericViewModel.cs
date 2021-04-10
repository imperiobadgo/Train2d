using Train2d.Model.Items;

namespace Train2d.Main.ViewModel.Items
{
  public abstract class ItemGenericViewModel<T> : ItemViewModel where T : Item
  {

    public T Item() => (T)BaseItem();
  }
}
