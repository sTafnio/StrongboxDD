using System.Linq;
using ExileCore;
using ExileCore.PoEMemory.Components;
using ExileCore.Shared.Enums;
using SharpDX;

namespace StrongboxDD;

public class StrongboxDD : BaseSettingsPlugin<StrongboxDDSettings>
{
    public override bool Initialise()
    {
        return true;
    }
    public override Job Tick()
    {
        return null;
    }

    public override void Render()
    {
        var detonateBoxes = GameController.EntityListWrapper.ValidEntitiesByType[EntityType.Chest]
            .Where(x => x.TryGetComponent<ObjectMagicProperties>(out var omp) &&
                    omp.Mods.Contains("ChestExplodeCorpses"));
                    
        foreach (var box in detonateBoxes)
        {
            var pos = box.PosNum;
            Graphics.DrawFilledCircleInWorld(pos, Settings.Radius, Color.Red);
        }
    }
}