using MoonGale.Core;

namespace MoonGale.Runtime.Indicators
{
    internal sealed class IndicatorTriggeredMessage : IMessage
    {
        public DangerSensor Sensor { get; }

        public IndicatorTriggeredMessage(DangerSensor sensor)
        {
            Sensor = sensor;
        }
    }

    internal sealed class IndicatorClearedMessage : IMessage
    {
        public DangerSensor Sensor { get; }

        public IndicatorClearedMessage(DangerSensor sensor)
        {
            Sensor = sensor;
        }
    }
}
