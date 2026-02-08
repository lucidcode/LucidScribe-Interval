using System;

namespace lucidcode.LucidScribe.Plugin.Interval
{
    public class PluginHandler : Interface.LucidPluginBase
    {
        private int interval = 40;
        private int lastSecond = -1;
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
            lastTrigger = DateTime.Now;
            return true;
        }

        public override double Value
        {
            get
            {
                if (DateTime.Now >= lastTrigger.AddSeconds(interval) &&
                    lastSecond != DateTime.Now.Second)
                {
                    lastTrigger = DateTime.Now;
                    lastSecond = DateTime.Now.Second;
                    return 888;
                }

                return 1;
            }
        }

    }

}