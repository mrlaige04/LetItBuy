using Domain.Intefaces.Composite;

namespace Domain.Entities;
public class Category : TreeComponent
{
    public string Name { get; set; } = null!;
    public Category(string name)
    {
        Name = name;
    }
}