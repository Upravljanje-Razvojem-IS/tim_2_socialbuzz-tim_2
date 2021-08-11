using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlockService.Logger
{
    public interface ILoggerRepository<T>
    {
        public void LogError(Exception ex, string message, params object[] args);

        public void LogInformation(string message);
    }
}
