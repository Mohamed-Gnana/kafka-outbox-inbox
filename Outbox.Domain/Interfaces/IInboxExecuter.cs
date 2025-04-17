using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outbox.Domain.Interfaces
{
    public interface IInboxExecuter
    {
        Task Execute(Func<Task> businessLogic);
    }
}
