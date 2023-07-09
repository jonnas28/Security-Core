using Identity.Context;

namespace Identity.Repository
{
    public interface IRepositoryWrapper
    {
        ApplicationContext GetContext();
        Task SaveAsync();
    }
}
