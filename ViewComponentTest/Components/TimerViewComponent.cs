using System;

namespace ViewComponentTest.Components
{
    public class TimerViewComponent
    {
        public string Invoke(bool includeSeconds = false, bool format24 = true)
        {
            string time = String.Empty;
            DateTime now = DateTime.Now;

            if (format24)
                time = now.ToString("HH:mm");
            else 
                time = now.ToString("hh:mm");

            if (includeSeconds)
                time = $"{time}:{now.Second}";

            return $"Текущее время: {time}";
        }
    }
}
