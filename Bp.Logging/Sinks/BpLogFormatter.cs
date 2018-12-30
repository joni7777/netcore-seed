using Serilog.Events;

namespace Bp.Logging.Sinks
{
    public abstract class BpLogFormatter
    {
        public abstract string Format(LogEvent log);
    }
}