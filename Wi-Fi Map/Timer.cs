using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Wi_Fi_Map
{
    public class Timer
    {
        private static Timer _uniq;
        public DispatcherTimer dispatcherTimer;
        private Timer()
        {
            this.dispatcherTimer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 10)
            };
        }

        public static Timer GetInstance()
        {
            if(_uniq==null)
            {
                _uniq = new Timer();
            }
            return _uniq;
        }
    }
}
