using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WoWItemCacheCreator.Classes
{
    class mytimer
    {
        internal void StartClock(ref Label l)
        {
            Timer t = new Timer();
            t.Interval = 1000;
            t.Enabled = true;
            t.Tag = l;
            t.Tick += new EventHandler(t_Tick);
            t.Start();
        }

        int i;
        bool stop;
        void t_Tick(object sender, EventArgs e)
        {
            if (stop)
            {
                ((Timer)sender).Stop();
                return;
            }
            
            var timespan = TimeSpan.FromSeconds((++i));
            

            ((Label)((Timer)sender).Tag).Text = timespan.ToString(@"hh\:mm\:ss");
        }

        internal void StopClock()
        {
            i = 0;
            stop = true;
        }
    }
}
