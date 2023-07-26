namespace Domain.Intefaces.Composite;
public abstract class TreeComponent : BaseAuditableEntity, ITreeComponent
{
    private ICollection<ITreeComponent> Children { get ; set; }
    public TreeComponent()
    {
        Children = new List<ITreeComponent>();
    }

    public void Add(ITreeComponent component)
    {
        Children.Add(component);
    }

    public void Remove(ITreeComponent component)
    {
        Children.Remove(component);
    }
}