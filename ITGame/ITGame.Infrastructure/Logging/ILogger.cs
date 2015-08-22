using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITGame.Infrastructure.Logging
{
    public interface ILogger
    {
        /// <summary>
        /// Logs Verbose level message
        /// </summary>
        /// <param name="message"></param>
        void LogStart(string message = null);

        /// <summary>
        /// Logs Verbose level message
        /// </summary>
        /// <param name="message"></param>
        void LogStop(string message = null);

        /// <summary>
        /// Logs Verbose level message
        /// </summary>
        /// <param name="message"></param>
        void LogVerbose(string message);

        /// <summary>
        /// Logs Information level message
        /// </summary>
        /// <param name="message"></param>
        void Log(string message);

        /// <summary>
        /// Logs Warning level message
        /// </summary>
        /// <param name="message"></param>
        void LogWarning(string message);

        /// <summary>
        /// Logs Error level message
        /// </summary>
        /// <param name="message"></param>
        void LogError(string message);

        /// <summary>
        /// Logs Error level message
        /// </summary>
        /// <param name="exception"></param>
        void LogError(Exception exception);

        /// <summary>
        /// Logs Error level message
        /// </summary>
        /// <param name="message">Message describing the <paramref name="exception"/></param>
        /// <param name="exception"></param>
        void LogError(string message, Exception exception);


    }
}
