namespace Domain.Intefaces.Composite;
public abstract class LeafComponent : ITreeComponent
{
    public void Add(ITreeComponent component) { }

    public void Remove(ITreeComponent component) { }
}