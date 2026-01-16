public class UISubItem : UserInterface
{
    public override void Init() => base.Init();

    public virtual void CloseSubItem() => Managers.Resource.Destroy(gameObject);
}
