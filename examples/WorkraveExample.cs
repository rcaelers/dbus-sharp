using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using DBus;
using org.freedesktop.DBus;
using org.workrave;

class WorkraveExample
{
    static Connection bus = null;

    public static int Main(string[] args)
    {
        bus = Bus.Session;

        ObjectPath corepath = new ObjectPath("/org/workrave/Workrave/Core");
        ObjectPath uipath = new ObjectPath("/org/workrave/Workrave/UI");
        string name = "org.workrave.Workrave";

        ICore core = bus.GetObject<ICore>(name, corepath);
        IConfig config = bus.GetObject<IConfig>(name, corepath);
        IControl control = bus.GetObject<IControl>(name, uipath);

        //control.ReadingMode(true);
        //control.RestBreak();
        //control.Statistics();

        //int v;
        //bool b = config.GetInt("timers/micro_pause/limit", out v);
        //config.SetInt("timers/micro_pause/limit", 201);
        //b = config.GetInt("timers/micro_pause/limit", out v);

        //string opmode = core.GetOperationMode();
        //core.SetOperationMode("normal");
        //opmode = core.GetOperationMode();
        //b = core.IsActive();
        //b = core.IsTimerRunning("microbreak");
        //int t = core.GetTime();
        //t = core.GetTimerElapsed("restbreak");
        //t = core.GetTimerIdle("restbreak");
        //t = core.GetTimerOverdue("restbreak");
        //core.PostponeBreak("microbreak");
        //core.SkipBreak("microbreak");

        core.MicrobreakChanged += new BreakChangedEventHandler(OnMicrobreakChanged);

        while (true)
        {
            bus.Iterate();
        }

        return 0;
    }

    static void OnMicrobreakChanged(string progress)
    {
        Console.WriteLine("Micro Break " + progress);
    }
}
