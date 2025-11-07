using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.CompilerServices;
using ExileCore;
using ExileCore.PoEMemory.Components;
using ExileCore.Shared.Enums;
using SharpDX;

namespace StrongboxDD;

public class StrongboxDD : BaseSettingsPlugin<StrongboxDDSettings>
{
    private List<long> _handledBoxes = new();
    private readonly string _soundFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Sounds", "alert.wav");

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
                    omp.Mods.Contains("ChestExplodeCorpses") &&
                    x.TryGetComponent<MinimapIcon>(out var mapIcon) &&
                    mapIcon.IsHide == false);


        foreach (var box in detonateBoxes)
        {
            if (Settings.PlaySound && !_handledBoxes.Contains(box.Address))
            {
                PlayNotificationSound();
                _handledBoxes.Add(box.Address);
            }


            if (Settings.DrawCircle)
            {
                var pos = box.PosNum;
                Graphics.DrawFilledCircleInWorld(pos, Settings.Radius, Color.Red);
            }
        }
    }

    public override void AreaChange(AreaInstance area)
    {
        _handledBoxes = new();
    }

    private void PlayNotificationSound()
    {
        if (!Settings.PlaySound)
        {
            return;
        }

        if (!File.Exists(_soundFilePath))
        {
            LogError("Sound File Not Found");
            return;
        }

        try
        {
            using var soundPlayer = new SoundPlayer(_soundFilePath);

            soundPlayer.Play();
        }
        catch (Exception ex)
        {
            LogError($"Sound Playback Failed: {ex.Message}");
        }
    }

}