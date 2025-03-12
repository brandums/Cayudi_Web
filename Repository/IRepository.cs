using System.Linq.Expressions;

namespace EPlatformWebApp.Repository
{
    public interface IRepository<T> where T : class
    {
        #region: Methods
        Task create(T model);
        Task <List<T>> getAll();

        Task<T> getFirstOrDefaultBy(Expression<Func<T, bool>> filter, bool tracked = true);

        Task <List<T>> getAllBy(Expression<Func<T, bool>> filter);

        Task <T> update(T model);

        Task Remove(T model);

        Task saveToDatabase();
        #endregion
    }
}
