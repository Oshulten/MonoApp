namespace MonoApp.Generic;

public static class Functions
{
    public static bool InRange(double value, double inclusiveMin, double inclusiveMax) =>
        value >= inclusiveMin && value <= inclusiveMax;

    public static double CycleInRange(double value, double min, double max) =>
        min + value % (max - min);
}