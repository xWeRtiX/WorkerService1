using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace WorkerService1
{
    public interface IBackupService
    {
        void Backup();
    }
}
