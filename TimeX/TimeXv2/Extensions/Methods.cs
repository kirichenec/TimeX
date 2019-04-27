using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TimeXv2.Model;

namespace TimeXv2.Extensions
{
    public static class Methods
    {
        #region ForEach<T>
        /// <summary>
        /// Allows an IEnumerable to iterate through its collection and perform an action
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
            {
                action(item);
            }
        }
        #endregion

        #region GetEndTime
        public static TimeSpan GetDuration(this ObservableCollection<Checkpoint> checkpoints)
        {
            var maxTime = new TimeSpan();
            checkpoints.ForEach(chk =>
            {
                var currTime = chk.StartTime + chk.Duration;
                if (currTime <= maxTime)
                {
                    return;
                }
                maxTime = currTime;
            });
            return maxTime;
        }
        #endregion
    }
}
