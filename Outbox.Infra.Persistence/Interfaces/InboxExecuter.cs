using Outbox.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outbox.Infra.Persistence.Interfaces
{
    public class InboxExecuter : IInboxExecuter
    {
        public Task Execute(Func<Task> businessLogic)
        {
            throw new NotImplementedException();
        }
    }
}
