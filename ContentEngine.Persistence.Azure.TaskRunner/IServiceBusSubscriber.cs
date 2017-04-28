using System;
using System.Collections.Generic;
using System.Text;

namespace ContentEngine.Persistence.Azure.TaskRunner
{
    public interface IServiceBusSubscriber
    {
        void BeginBackgroundProcessing();
    }
}
