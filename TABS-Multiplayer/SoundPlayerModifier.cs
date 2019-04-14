using System.Collections;
using TABS_Multiplayer;
using UnityEngine;

#pragma warning disable CS0626
class patch_SoundPlayer : SoundPlayer
{
    public extern IEnumerator orig_Play(SoundEffectInstance soundEffect, AudioPlayer player, float volumeMultiplier,
        Vector3 position, SoundEffectVariations.MaterialType materialType, Transform transformToFollow);

    public IEnumerator Play(SoundEffectInstance soundEffect, AudioPlayer player, float volumeMultiplier,
        Vector3 position, SoundEffectVariations.MaterialType materialType, Transform transformToFollow) // Hook this method for audio sharing
    {
        if(SocketConnection.GetIsServer() && SocketConnection.gameStarted)
        {
            SocketConnection.SetCulture();
            Vector3 distance = position == Vector3.zero ? position : Camera.main.transform.position - position; // Get the relative position from the camera
            string tosend = "AUDIO|" + GetCategory(soundEffect) + "/" + soundEffect.soundRef + "|" + 
                volumeMultiplier + "|" + distance.ToString("F5") + "|" + materialType.ToString();
            SocketConnection.WriteToOpponent(tosend); // Transfer the sound
        }
        return orig_Play(soundEffect, player, volumeMultiplier, position, materialType, transformToFollow);
    }

    private string GetCategory(SoundEffectInstance sound) // Get the respective soundRef categoryName
    {
        foreach(SoundBankCategory cat in soundBank.Categories)
        {
            foreach(SoundEffectInstance sei in cat.soundEffects)
            {
                if (sound.category == sei.category && sound.soundRef == sei.soundRef)
                    return cat.categoryName;
            }
        }
        return "";
    }
}