using System;
using System.Drawing;
using ExileCore.Shared.Interfaces;
using ExileCore.Shared.Nodes;

namespace StrongboxDD;

public class StrongboxDDSettings : ISettings
{
    //Mandatory setting to allow enabling/disabling your plugin
    public ToggleNode Enable { get; set; } = new ToggleNode(false);

    public RangeNode<float> Radius { get; set; } = new(82, 0, 400);
}