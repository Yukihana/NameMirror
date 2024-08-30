using System.Collections.Generic;

namespace RNGui.Resources.HardCoded;

public static class InstructionsData
{
    public static List<KeyValuePair<string, string>> Tasks => new()
    {
        new KeyValuePair<string, string>(
            "Add Task(s)",
            "Adds files to be renamed. New tasks are added at the end of the list."),
        new KeyValuePair<string, string>(
            "Insert Task(s)",
            "Adds files to be renamed. New tasks are added at the selected row, shifting existing rows downwards.")
    };

    public static List<KeyValuePair<string, string>> References => new() { };
    public static List<KeyValuePair<string, string>> Renaming => new() { };
    public static List<KeyValuePair<string, string>> Tools => new() { };
    public static List<KeyValuePair<string, string>> Log => new() { };
    public static List<KeyValuePair<string, string>> Misc => new() { };
    public static List<KeyValuePair<string, string>> Overview => new() { };
}