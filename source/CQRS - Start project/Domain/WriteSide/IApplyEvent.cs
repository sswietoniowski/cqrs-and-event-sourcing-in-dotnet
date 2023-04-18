namespace Domain.WriteSide;

public interface IApplyEvent<in TEvent>
{
    void Apply(TEvent e);
}