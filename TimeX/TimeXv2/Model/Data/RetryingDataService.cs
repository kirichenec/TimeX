using System;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace TimeXv2.Model.Data
{
    public class RetryingDataService<T, TV> : IRetryingDataService<T, TV>
    {
        #region ctor
        public RetryingDataService()
        {
            RetryDelay = TimeSpan.FromSeconds(2);
        }

        public RetryingDataService(TimeSpan retryDelay)
        {
            RetryDelay = retryDelay;
        }
        #endregion

        #region Properties

        #region RetryDelay
        private TimeSpan _retryDelay;

        public TimeSpan RetryDelay
        {
            get { return _retryDelay; }
            set { _retryDelay = value; }
        }
        #endregion

        #endregion

        #region Methods

        #region RunTheMethod
        public async Task<DataResult<T>> RunTheMethod(Func<TV, Task<T>> actionGenericAsync, TV value, byte retryCount = 2)
        {
            var answer = new DataResult<T>();

            while (retryCount > 0)
            {
                try
                {
                    answer.Result = await actionGenericAsync(value);
                    return answer;
                }
                catch (EntityException dbException)
                {
                    var sqliteException = dbException.InnerException as SQLiteException;
                    answer.Message = sqliteException.Message;
                }
                catch (DbUpdateConcurrencyException exception)
                {
                    answer.Message = exception.Message;
                    return answer;
                }
                catch (Exception exception)
                {
                    answer.Message = exception.Message;
                    return answer;
                }
                retryCount--;
                await Task.Delay(RetryDelay);
            }

            return answer;
        }

        public async Task<DataResult<T>> RunTheMethod(Func<Task<T>> actionGenericAsync, byte retryCount = 2)
        {
            var answer = new DataResult<T>();

            while (retryCount > 0)
            {
                try
                {
                    answer.Result = await actionGenericAsync();
                    return answer;
                }
                catch (EntityException dbException)
                {
                    var sqliteException = dbException.InnerException as SQLiteException;
                    answer.Message = sqliteException.Message;
                }
                catch (DbUpdateConcurrencyException exception)
                {
                    answer.Message = exception.Message;
                    return answer;
                }
                catch (Exception exception)
                {
                    answer.Message = exception.Message;
                    return answer;
                }
                retryCount--;
                await Task.Delay(RetryDelay);
            }

            return answer;
        }
        #endregion

        #endregion
    }
}