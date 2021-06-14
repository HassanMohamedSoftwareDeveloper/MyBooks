using MyBooks.MyBooks.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace MyBooks.MyBooks.Data.Services
{
    public class LogsService
    {
        private readonly AppDbContext _appDbContext;
        public LogsService(AppDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
        }

        public List<Log> GetAllLogs() => _appDbContext.Logs.ToList();
    }
}
