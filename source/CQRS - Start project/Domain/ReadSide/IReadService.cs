namespace Domain.ReadSide
{
    public interface IReadService
    {
        TResult Query<TProjection, TResult>(Func<TProjection, TResult> query)
            where TProjection : Projection, new();
    }
}
