using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using ExileCore;
using ExileCore.PoEMemory.Components;
using ExileCore.Shared.Enums;
using SharpDX;

namespace StrongboxDD;

public class StrongboxDD : BaseSettingsPlugin<StrongboxDDSettings>
{
    private List<long> _handledBoxes = [];
    private List<long> _handledAltars = [];
    private readonly string _eaterAltarMetadata = "Metadata/MiscellaneousObjects/PrimordialBosses/TangleAltar";
    private readonly string _strongboxSoundFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Sounds", "alert.wav");
    private readonly string _divineAltarSoundFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Sounds", "treasure.wav");


    public override bool Initialise()
    {
        Name = "Special Helper";
        _handledBoxes = [];
        _handledAltars = [];

        return true;
    }

    public override Job Tick()
    {
        return null;
    }

    public override void Render()
    {
        var detonateBoxes = GameController.EntityListWrapper.ValidEntitiesByType[EntityType.Chest]
            .Where(x => x.DistancePlayer <= 150 &&
                    x.TryGetComponent<ObjectMagicProperties>(out var omp) &&
                    omp.Mods.Contains("ChestExplodeCorpses") &&
                    x.TryGetComponent<MinimapIcon>(out var mapIcon) &&
                    mapIcon.IsHide == false);


        foreach (var box in detonateBoxes)
        {
            if (Settings.PlayDDStrongboxSound && !_handledBoxes.Contains(box.Address))
            {
                PlayNotificationSound(_strongboxSoundFilePath);
                _handledBoxes.Add(box.Address);
            }

            if (Settings.DrawDDStrongboxCircle)
            {
                var pos = box.PosNum;
                Graphics.DrawFilledCircleInWorld(pos, Settings.DDStrongboxRadius, Color.Red);
            }
        }

        var divineAltars = GameController.IngameState.IngameUi.ItemsOnGroundLabelsVisible
            .Where(x => x.ItemOnGround.Metadata == _eaterAltarMetadata &&
                    (x.Label.GetChildFromIndices(0, 1).Text.Contains("Divine") ||
                    x.Label.GetChildFromIndices(1, 1).Text.Contains("Divine")));

        foreach (var altar in divineAltars)
        {
            if (Settings.PlayDivineAltarSound && !_handledAltars.Contains(altar.Address))
            {
                PlayNotificationSound(_divineAltarSoundFilePath);
                _handledAltars.Add(altar.Address);
            }

            if (Settings.DrawDivineAltarCircle)
            {
                var pos = altar.ItemOnGround.PosNum;
                Graphics.DrawFilledCircleInWorld(pos, Settings.DivineAltarRadius, Color.Green);
            }
        }

    }

    public override void AreaChange(AreaInstance area)
    {
        _handledBoxes = [];
        _handledAltars = [];
    }

    private void PlayNotificationSound(string path)
    {
        if (!Settings.PlayDDStrongboxSound)
        {
            return;
        }

        if (!File.Exists(path))
        {
            LogError("Sound File Not Found");
            return;
        }

        try
        {
            LogMessage("PLAYING SOUND");
            using var soundPlayer = new SoundPlayer(path);

            soundPlayer.Play();
        }
        catch (Exception ex)
        {
            LogError($"Sound Playback Failed: {ex.Message}");
        }
    }

}