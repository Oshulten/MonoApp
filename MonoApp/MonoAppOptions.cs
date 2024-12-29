using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonoApp;
public record MonoAppOptions
{
    public int Width { get; set; } = 800;
    public int Height { get; set; } = 500;
    public bool IsFullscreen { get; set; } = false;
    public string WindowTitle { get; set; } = "Window Title";
}