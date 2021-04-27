using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class YandexCustomEvent
{
    public static void LevelStart(int level_number)
    {
        var tutParms = new Dictionary<string, object>();
        //tutParms["level_loop"] = level_loop;
        tutParms["level_number"] = level_number;

        AppMetrica.Instance.ReportEvent("level_start", tutParms);
        AppMetrica.Instance.SendEventsBuffer();
    }
    public static void LevelFinish(int level_number)
    {
        var tutParms = new Dictionary<string, object>();
        //tutParms["level_loop"] = level_loop;
        tutParms["level_number"] = level_number;

        AppMetrica.Instance.ReportEvent("level_loop", tutParms);
        AppMetrica.Instance.SendEventsBuffer();
    }
}
