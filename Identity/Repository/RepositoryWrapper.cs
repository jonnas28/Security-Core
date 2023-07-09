using Identity.Context;

namespace Identity.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        ApplicationContext _context;
        public RepositoryWrapper( ApplicationContext context )
        {
            _context = context;
        }

        public ApplicationContext GetContext()
        {
            return _context;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
