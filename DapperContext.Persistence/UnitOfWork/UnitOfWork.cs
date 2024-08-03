using DapperContext.Application.UnitOfWork;
using DapperContext.Persistence.Context;

namespace DapperContext.Persistence.UnitOfWork
{
    public class UnitOfWork(ContextDapper contextDapper) : IUnitOfWork
    {
        private readonly ContextDapper _contextDapper = contextDapper ?? throw new ArgumentNullException(nameof(contextDapper));

        public async Task BeginTransaction(Func<CancellationToken, Task> func, CancellationToken cancellationToken)
        {
            _contextDapper.Connection!.Open();
            _contextDapper.Transaction = _contextDapper.Connection.BeginTransaction();
            try
            {
                await func(cancellationToken);
                Commit();
            }
            catch
            {
                Rollback();
                throw;
            }
            finally
            {
                if (_contextDapper.Transaction != null && _contextDapper.Connection != null)
                {
                    _contextDapper.Transaction.Dispose();
                    _contextDapper.Connection.Close();
                }
            }
        }

        public void Commit()
        {
            try
            {
                _contextDapper.Transaction!.Commit();
            }
            catch
            {
                _contextDapper.Transaction!.Rollback();
                throw;
            }
            finally
            {
                _contextDapper.Transaction?.Dispose();
                _contextDapper.Connection!.Close();
            }
        }

        public void Rollback()
        {
            _contextDapper.Transaction?.Rollback();
            _contextDapper.Transaction?.Dispose();
            _contextDapper.Connection!.Close();
        }
    }
}