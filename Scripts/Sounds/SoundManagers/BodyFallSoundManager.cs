using UnityEngine;

public class BodyFallSoundManager : SoundManager
{
    private bool _wasPlayed = false;
    
    private void OnCollisionEnter(Collision other)
    {
        if (!_wasPlayed)
        {
            _wasPlayed = true;
            PlayClipWithRandomVolumeAndPitch();
        }
    }
}
