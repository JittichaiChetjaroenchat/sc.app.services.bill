using System.Threading.Tasks;

namespace SC.App.Services.Bill.Lib.Extensions
{
    /// <summary>
    /// The async extension
    /// </summary>
    public static class AsyncExtension
    {
        /// <summary>
        /// Run sync
        /// </summary>
        /// <typeparam name="T">The type of result</typeparam>
        /// <param name="task">The task</param>
        /// <returns>The result</returns>
        public static T RunSync<T>(this Task<T> task)
        {
            return task
                .GetAwaiter()
                .GetResult();
        }
    }
}