using DBus;

namespace org.workrave
{
    public delegate void BreakChangedEventHandler(string progress);

    [Interface("org.workrave.CoreInterface")]
    public interface ICore
    {
        void SetOperationMode(string mode);
        string GetOperationMode();

        void ReportActivity(string who, bool act);
        bool IsTimerRunning(string timer_id);
        int GetTimerIdle(string timer_id);
        int GetTimerElapsed(string timer_id);
        int GetTimerOverdue(string timer_id);

        int GetTime();
        bool IsActive();

        void PostponeBreak(string timer_id);
        void SkipBreak(string timer_id);

        event BreakChangedEventHandler MicrobreakChanged;
        event BreakChangedEventHandler RestbreakChanged;
        event BreakChangedEventHandler DailylimitChanged;
    }

    [Interface("org.workrave.ConfigInterface")]
    public interface IConfig
    {
        bool SetString(string key, string value);
        bool SetInt(string key, int value);
        bool SetBool(string key, bool value);
        bool SetDouble(string key, double value);

        bool GetString(string key, out string value);
        bool GetInt(string key, out int value);
        bool GetBool(string key, out bool value);
        bool GetDouble(string key, out double value);
    }

    [Interface("org.workrave.ControlInterface")]
    public interface IControl
    {
        void OpenMain();
        void Preferences();
        void NetworkConnect();
        void NetworkLog(bool show);
        void NetworkReconnect();
        void NetworkDisconnect();
        void ReadingMode(bool on);
        void Statistics();
        void Exercises();
        void RestBreak();
        void Quit();
        void About();
    }
}

