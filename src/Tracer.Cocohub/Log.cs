using Logger.Cocohub;
using System;

namespace Tracer.Cocohub
{
    public static class Log 
    {
        private static string _timeFormat = "yyyy-MM-dd HH:mm:ss.fff";
        private static string _text = "[{0}] [{1}] {2}";

        private static string Format(string message, string level)
        {
            return string.Format(_text, DateTime.Now.ToString(_timeFormat), level, message);
        }

        /// <overloads>Log a message with the <see cref="Level.Debug"/> level.</overloads>
		/// <summary>
		/// Log a message with the <see cref="Level.Debug"/> level.
		/// </summary>
		/// <param name="message">The message to log.</param>
        public static void Debug(string message) { LogCollector.Add(Format(message, "Debug")); }

        /// <overloads>Log a message with the <see cref="Level.Info"/> level.</overloads>
        /// <summary>
        /// Logs a message with the <see cref="Level.Info"/> level.
        /// </summary>
        public static void Info(string message) { LogCollector.Add(Format(message, "Info")); }

        /// <overloads>Log a message with the <see cref="Level.Warn"/> level.</overloads>
        /// <summary>
        /// Log a message with the <see cref="Level.Warn"/> level.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void Warn(string message) { LogCollector.Add(Format(message, "Warn")); }

        /// <overloads>Log a message with the <see cref="Level.Error"/> level.</overloads>
        /// <summary>
        /// Logs a message with the <see cref="Level.Error"/> level.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void Error(string message) { LogCollector.Add(Format(message, "Error")); }

        /// <overloads>Log a message with the <see cref="Level.Fatal"/> level.</overloads>
        /// <summary>
        /// Log a message with the <see cref="Level.Fatal"/> level.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void Fatal(string message) { LogCollector.Add(Format(message, "Fatal")); }

    }
}
