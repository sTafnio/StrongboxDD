using System;
using System.Drawing;
using ExileCore.Shared.Interfaces;
using ExileCore.Shared.Nodes;

namespace StrongboxDD;

public class StrongboxDDSettings : ISettings
{
    //Mandatory setting to allow enabling/disabling your plugin
    public ToggleNode Enable { get; set; } = new ToggleNode(false);
    public ToggleNode PlayDDStrongboxSound { get; set; } = new(false);
    public ToggleNode DrawDDStrongboxCircle { get; set; } = new(true);
    public RangeNode<float> DDStrongboxRadius { get; set; } = new(82, 0, 400);
    public ToggleNode PlayDivineAltarSound { get; set; } = new(false);
    public ToggleNode DrawDivineAltarCircle { get; set; } = new(true);
    public RangeNode<float> DivineAltarRadius { get; set; } = new(82, 0, 400);
}