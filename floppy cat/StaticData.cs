using Avalonia.Controls;
using floppy_cat.Views;

namespace floppy_cat;

public static class StaticData
{
    public static Floppy floppy;
    public static Canvas canvas;
    public static MainMenu mainMenu;
    public static Window currentWindow;
    public static float speedMult = 1;
}