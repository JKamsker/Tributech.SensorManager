using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tributech.SensorManager.Application.Sensors.Common;

internal static class CacheKeys
{
    public static string SensorsList => "sensors";

    public static string SensorItem(Guid id) => $"sensor_{id}";

    public static string SensorItem(string id) => $"sensor_{id}";
}