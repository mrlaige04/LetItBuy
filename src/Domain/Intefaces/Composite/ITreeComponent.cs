namespace Domain.Intefaces.Composite;
public interface ITreeComponent
{
    void Add(ITreeComponent component);
    void Remove(ITreeComponent component);
}