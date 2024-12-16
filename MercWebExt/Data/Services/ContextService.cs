using MercWebExt.Models.DataBase;
using Microsoft.Extensions.Configuration;

namespace MercWebExt.Services
{
    public interface IContextService
    {
        DatabaseContext GetDataBaseContext();
    }

    public class ContextService : IContextService
    {
        private readonly DatabaseContext _dbContext;

        // Inject DatabaseContext through constructor
        public ContextService(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DatabaseContext GetDataBaseContext()
        {
            return _dbContext;
        }
    }
}
