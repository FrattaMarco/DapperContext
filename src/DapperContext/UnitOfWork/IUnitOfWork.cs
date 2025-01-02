namespace DapperContext.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task BeginTransaction(Func<CancellationToken, Task> function, CancellationToken cancellationToken);
        void Commit();
        void Rollback();
    }
}
