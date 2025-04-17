using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Broker
{
    public interface IPublisher
    {
        Task Publish<T> (T message, Guid? messageId = null, Guid? traceId = null) where T : class;
    }
}
