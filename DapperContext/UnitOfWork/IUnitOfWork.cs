namespace DapperContext.Application.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task BeginTransaction(Func<CancellationToken, Task> func, CancellationToken cancellationToken);
        void Commit();
        void Rollback();
    }
}
