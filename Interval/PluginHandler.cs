using System;
using System.Drawing;
using System.Linq;

namespace lucidcode.LucidScribe.Plugin.Interval
{
    public static class Device
    {
        public static EventHandler<EventArgs> ValueChanged;

        static bool initialized;
        private static int delay = 40;
        private static int lastSecond = -1;
        private static DateTime lastTrigger;

        private static System.Threading.Timer timer;

        static int interval;

        public static Boolean Initialize()
        {
            if (initialized)
            {
                return true;
            }
            initialized = true;
            timer = new System.Threading.Timer(Tick, "interval", TimeSpan.FromMilliseconds(10), TimeSpan.FromMilliseconds(500));
            return true;
        }

        private static void Tick(object state)
        {
            if (lastSecond == DateTime.Now.Second)
            {
                return;
            }

            if (DateTime.Now >= lastTrigger.AddSeconds(delay) &&
                lastSecond != DateTime.Now.Second)
            {
                lastTrigger = DateTime.Now;
                lastSecond = DateTime.Now.Second;
                interval = 888;
            }
            else
            {
                interval = 1;
            }

            string values = string.Join(",", Enumerable.Repeat(interval, 512));

            if (ValueChanged != null)
            {
                ValueChanged((object)values, null);
            }
        }

        public static void Dispose()
        {
            timer.Dispose();
        }

        public static Double GetInterval()
        {
            return interval;
        }
    }

    namespace Timer
    {
        public class PluginHandler : Interface.LucidPluginBase
        {
            private DateTime lastTrigger;

            public override string Name
            {
                get
                {
                    return "Interval REM";
                }
            }

            public override bool Initialize()
            {
                try
                {
                    return Device.Initialize();
                }
                catch (Exception ex)
                {
                    throw (new Exception("The '" + Name + "' plugin failed to initialize: " + ex.Message));
                }
            }

            public override double Value
            {
                get
                {
                    double value = Device.GetInterval();
                    if (value == 888 && DateTime.Now >= lastTrigger.AddSeconds(3))
                    {
                        lastTrigger = DateTime.Now;
                        return value;
                    }
                    return 1;
                }
            }

            public override void Dispose()
            {
                Device.Dispose();
            }
        }
    }

    namespace RAW
    {
        public class PluginHandler : Interface.ILluminatedPlugin
        {
            public string Name
            {
                get
                {
                    return "Interval RAW";
                }
            }

            public bool Initialize()
            {
                try
                {
                    bool initialized = Device.Initialize();
                    Device.ValueChanged += ValueChanged;
                    return initialized;
                }
                catch (Exception ex)
                {
                    throw (new Exception("The '" + Name + "' plugin failed to initialize: " + ex.Message));
                }
            }

            public event Interface.SenseHandler Sensed;
            public void ValueChanged(object sender, EventArgs e)
            {
                if (ClearTicks)
                {
                    ClearTicks = false;
                    TickCount = "";
                }
                TickCount += sender + ",";

                if (ClearBuffer)
                {
                    ClearBuffer = false;
                    BufferData = "";
                }
                BufferData += sender + ",";
            }

            public void Dispose()
            {
                Device.ValueChanged -= ValueChanged;
                Device.Dispose();
            }

            public Boolean isEnabled = false;
            public Boolean Enabled
            {
                get
                {
                    return isEnabled;
                }
                set
                {
                    isEnabled = value;
                }
            }

            public Color PluginColor = Color.White;
            public Color Color
            {
                get
                {
                    return Color;
                }
                set
                {
                    Color = value;
                }
            }

            private Boolean ClearTicks = false;
            public String TickCount = "";
            public String Ticks
            {
                get
                {
                    ClearTicks = true;
                    return TickCount;
                }
                set
                {
                    TickCount = value;
                }
            }

            private Boolean ClearBuffer = false;
            public String BufferData = "";
            public String Buffer
            {
                get
                {
                    ClearBuffer = true;
                    return BufferData;
                }
                set
                {
                    BufferData = value;
                }
            }

            int lastHour;
            public int LastHour
            {
                get
                {
                    return lastHour;
                }
                set
                {
                    lastHour = value;
                }
            }
        }
    }
}
