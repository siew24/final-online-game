using System;
using System.Collections.Generic;

namespace Constants
{
    public enum Area
    {
        Area0,
        Area1,
        Area2,
        Area3
    }

    public static class Utils
    {
        public static string GetHintId(Area area, int hintNumber)
        {
            return Enum.GetName(typeof(Area), area) + $"/HINT_{hintNumber}";
        }
    }

    public static class Area0
    {
        public static int COUNT = 2;
        public static Dictionary<int, string> HINT = new() {
            {1, nameof(Area0) + "/HINT_1"},
            {2, nameof(Area0) + "/HINT_2"}
        };
    }

    public static class Area1
    {
        public static int COUNT = 3;
        public static Dictionary<int, string> HINT = new() {
            {1, nameof(Area1) + "/HINT_1"},
            {2, nameof(Area1) + "/HINT_2"},
            {3, nameof(Area1) + "/HINT_3"}
        };
    }

    public static class Area2
    {
        public static int COUNT = 4;
        public static Dictionary<int, string> HINT = new() {
            {1, nameof(Area2) + "/HINT_1"},
            {2, nameof(Area2) + "/HINT_2"},
            {3, nameof(Area2) + "/HINT_3"},
            {4, nameof(Area2) + "/HINT_4"}
        };
    }

    public static class Area3
    {
        public static int COUNT = 5;
        public static Dictionary<int, string> HINT = new() {
            {1, nameof(Area3) + "/HINT_1"},
            {2, nameof(Area3) + "/HINT_2"},
            {3, nameof(Area3) + "/HINT_3"},
            {4, nameof(Area3) + "/HINT_4"},
            {5, nameof(Area3) + "/HINT_5"}
        };
    }
}