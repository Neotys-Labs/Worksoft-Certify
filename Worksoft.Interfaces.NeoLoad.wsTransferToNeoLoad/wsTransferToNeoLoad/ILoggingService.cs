using System;

namespace wsTransferToNeoLoad
{
    public interface ILoggingService
    {
        /// <summary>
        ///     logs the specified message to the Debug logging level
        /// </summary>
        /// <param name="message">The message.</param>
        void Debug(object message);

        /// <summary>
        ///     logs the specified message to the Debug logging level
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        void Debug(object message, Exception exception);

        /// <summary>
        ///     logs the specified message to the Debug logging level
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        void DebugFormat(string format, object arg0);

        /// <summary>
        ///     logs the specified message to the Debug logging level
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        /// <param name="arg1">The arg1.</param>
        void DebugFormat(string format, object arg0, object arg1);

        /// <summary>
        ///     logs the specified message to the Debug logging level
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        /// <param name="arg1">The arg1.</param>
        /// <param name="arg2">The arg2.</param>
        void DebugFormat(string format, object arg0, object arg1, object arg2);

        /// <summary>
        ///     logs the specified message to the Debug logging level
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        void DebugFormat(string format, params object[] args);

        /// <summary>
        ///     logs the specified message to the Info logging level
        /// </summary>
        /// <param name="message">The message.</param>
        void Info(object message);

        /// <summary>
        ///     logs the specified message to the Info logging level
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        void Info(object message, Exception exception);

        /// <summary>
        ///     logs the specified message to the Error logging level
        /// </summary>
        /// <param name="message">The message.</param>
        void Error(object message);

        /// <summary>
        ///     logs the specified message to the Error logging level
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        void Error(object message, Exception exception);

        /// <summary>
        ///     logs the specified message to the Error logging level
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        void ErrorFormat(string format, object arg0);

        /// <summary>
        ///     logs the specified message to the Error logging level
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        /// <param name="arg1">The arg1.</param>
        void ErrorFormat(string format, object arg0, object arg1);

        /// <summary>
        ///     logs the specified message to the Error logging level
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        /// <param name="arg1">The arg1.</param>
        /// <param name="arg2">The arg2.</param>
        void ErrorFormat(string format, object arg0, object arg1, object arg2);

        /// <summary>
        ///     logs the specified message to the Error logging level
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        void ErrorFormat(string format, params object[] args);

        /// <summary>
        ///     logs the specified message to the Warn logging level
        /// </summary>
        /// <param name="message">The message.</param>
        void Warn(object message);

        /// <summary>
        ///     logs the specified message to the Warn logging level
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        void Warn(object message, Exception exception);
    }
}