using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace RanterTools
{
    public class ToolsDebug
    {
        [Conditional("RANTER_TOOLS_DEBUG")]
        public static void Log(object log)
        {
            Debug.Log(log);
        }
        
        [Conditional("RANTER_TOOLS_DEBUG")]
        public static void Log(object log, Object context)
        {
            Debug.Log(log,context);
        }

        [Conditional("RANTER_TOOLS_DEBUG")]
        public static void LogError(object log)
        {
            Debug.LogError(log);
        }
        
        [Conditional("RANTER_TOOLS_DEBUG")]
        public static void LogError(object log, Object context)
        {
            Debug.LogError(log,context);
        }

        [Conditional("RANTER_TOOLS_DEBUG")]
        public static void LogWarning(object log)
        {
            Debug.Log(log);
        }
        
        [Conditional("RANTER_TOOLS_DEBUG")]
        public static void LogWarning(object log, Object context)
        {
            Debug.Log(log,context);
        }
    }
}
