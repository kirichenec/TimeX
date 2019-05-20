using System;
using System.Threading.Tasks;

namespace TimeXv2.Model.Data
{
    interface IRetryingDataService<T, TV>
    {
        Task<DataResult<T>> RunTheMethod(Func<Task<T>> actionGenericAsync, byte retryCount = 2);
        Task<DataResult<T>> RunTheMethod(Func<TV, Task<T>> addActionGenericAsync, TV value, byte retryCount = 2);
    }
}
