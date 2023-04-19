namespace Domain.ReadSide;

public interface IBuildFrom<TEvent>
{
    void Apply(TEvent e);
}