using Cocohub.Tracer.Fody.Helpers;
using Cocohub.Tracer.Fody.Weavers;
using Mono.Cecil;
using System;
using System.Xml.Linq;

namespace Cocohub.Tracer.Fody
{
    /// <summary>
    /// Class that links fody to the real weaver
    /// </summary>
    public class ModuleWeaver : IWeavingLogger
    {
        public ModuleDefinition ModuleDefinition { get; set; }

        /// <summary>
        /// Weaves the tracer to a the module specified in <see cref="ModuleDefinition"/> property. It adds a trace enter and trace leave call to all methods defined by the filter.
        /// It also replaces static Log calls to logger instance calls and extends the call parameters with method name information.
        /// It uses the configuration to identify the exact weaver behavior.  
        /// </summary>
        public void Execute()
        {
            WeavingLog.SetLogger(this);

            var parser = FodyConfigParser.Parse(Config);

            if (parser.IsErroneous)
            {
                LogError(parser.Error);
            }
            else
            {
                ModuleLevelWeaver.Execute(parser.Result, ModuleDefinition);
            }
        }

        private TraceLoggingConfiguration ParseConfiguration(XElement config)
        {
            return null;
        }

        // Will contain the full element XML from FodyWeavers.xml. OPTIONAL
        public XElement Config { get; set; }

        // Will log an MessageImportance.Normal message to MSBuild. OPTIONAL
        public Action<string> LogDebug { get; set; }

        // Will log an MessageImportance.High message to MSBuild. OPTIONAL
        public Action<string> LogInfo { get; set; }

        // Will log an warning message to MSBuild. OPTIONAL
        public Action<string> LogWarning { get; set; }

        // Will log an error message to MSBuild. OPTIONAL
        public Action<string> LogError { get; set; }

        void IWeavingLogger.LogDebug(string message)
        {
            LogDebug(message);
        }

        void IWeavingLogger.LogInfo(string message)
        {
            LogInfo(message);
        }

        void IWeavingLogger.LogWarning(string message)
        {
            LogWarning(message);
        }

        void IWeavingLogger.LogError(string message)
        {
            LogError(message);
        }
    }
}
