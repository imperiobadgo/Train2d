using Train2d.Model.Items;

namespace Train2d.Main.ViewModel.Items
{
  public class ItemGenericViewModel<T> : ItemViewModel where T : Item
  {

    public T Item() => (T)BaseItem();
  }
}
