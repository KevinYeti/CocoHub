using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics;
using System.Text;
using Tracer.Cocohub.Context;

namespace Tracer.Cocohub.Adapters
{
    public class LoggerAdapter
    {
        private const string NullString = "<NULL>";
        private readonly Type _type;
        private static string _textEnter = "{{\"Action\":\"Enter\",\"Method\":\"{0}\",\"Params\":\"{1}\"}}";
        private static string _textReturn = "{{\"Action\":\"Return\",\"Method\":\"{0}\",\"Result\":\"{1}\",\"Time\":{2}}}";
        private static string _textEnterWithTracer = "{{\"Action\":\"Enter\",\"Method\":\"{0}\",\"Params\":\"{1}\",\"TracerId\":\"{2}\",\"SpanId\":\"{3}\"}}";
        private static string _textReturnWithTracer = "{{\"Action\":\"Return\",\"Method\":\"{0}\",\"Result\":\"{1}\",\"Time\":{2},\"TracerId\":\"{3}\",\"SpanId\":\"{4}\"}}";

        public LoggerAdapter(Type type)
        {
            _type = type;
        }

        #region Methods required for trace enter and leave

        public void TraceEnter(string methodInfo, string[] paramNames, object[] paramValues)
        {
            string argInfo = string.Empty;
            if (paramNames != null)
            {
                StringBuilder parameters = new StringBuilder();
                for (int i = 0; i < paramNames.Length; i++)
                {
                    parameters.AppendFormat("{0}={1}", paramNames[i], 
                        paramValues[i] == null ? NullString : paramValues[i].ToString().Replace("\r\n", string.Empty).Replace("\"", string.Empty));
                    if (i < paramNames.Length - 1) parameters.Append(", ");
                }
                argInfo = parameters.ToString();
            }

            TracerContext.Enter();
            string message = String.Empty;
            if (TracerContext.Tracer == null)
                message = String.Format(_textEnter, methodInfo, argInfo);
            else
                message = String.Format(_textEnterWithTracer, methodInfo, argInfo, TracerContext.Tracer.TracerId, TracerContext.Tracer.SpanId);

            Log.Info(message);
        }

        public void TraceLeave(string methodInfo, long startTicks, long endTicks, string[] paramNames,
            object[] paramValues)
        {
            string returnValue = null;

            if (paramNames != null)
            {
                StringBuilder parameters = new StringBuilder();
                for (int i = 0; i < paramNames.Length; i++)
                {
                    parameters.AppendFormat("{0}={1}", paramNames[i] ?? "$return", 
                        paramValues[i] == null ? NullString : paramValues[i].ToString().Replace("\r\n", string.Empty).Replace("\"", string.Empty));
                    if (i < paramNames.Length - 1) parameters.Append(", ");
                }
                returnValue = parameters.ToString();
            }

            double timeTaken = ConvertTicksToMilliseconds(endTicks - startTicks);

            string message = String.Empty;
            if (TracerContext.Tracer == null)
                message = String.Format(_textReturn, methodInfo, returnValue, timeTaken);
            else
                message = String.Format(_textReturnWithTracer, methodInfo, returnValue, timeTaken, TracerContext.Tracer.TracerId, TracerContext.Tracer.SpanId);

            Log.Info(message);
            TracerContext.Leave();
        }

        #endregion

        private static double ConvertTicksToMilliseconds(long ticks)
        {
            // ms = ticks * tickFrequency * 10000
            double ms = ticks * (10000000 / (double)Stopwatch.Frequency) / 10000L;
            return (int)(ms * 10) / 10.0D;
        }
    }
}