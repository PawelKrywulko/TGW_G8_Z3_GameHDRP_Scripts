using UnityEngine;
using UnityEngine.UI;

public class Mana : MonoBehaviour
{
    [SerializeField] private Slider manaBar = default;
    [SerializeField] private float maxMana = default;

    [Header("Voice Over")] 
    [SerializeField] private AudioSource voiceOverAudioSource;
    [SerializeField] private VoiceOverData voiceOverData;

    public float AvailableMana { get; private set; }

    private void Start()
    {
        AvailableMana = maxMana;
        manaBar.maxValue = maxMana;
        manaBar.value = AvailableMana;
    }

    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            RestoreMaxMana();
        }
    }*/

    public void RestoreMaxMana()
    {
        AvailableMana = maxMana;
        manaBar.maxValue = maxMana;
        manaBar.value = AvailableMana;
        _canPlayVo = true;
    }
    
    public void ConsumeMana(int manaCost)
    {
        if (AvailableMana <= 0) return;
        AvailableMana -= manaCost;
        manaBar.value = AvailableMana;
        CheckConditionsForVoiceOver();
    }

    public bool RechargeMana(float incomingMana)
    {
        if (AvailableMana == maxMana)
        {
            _canPlayVo = true;
            return false;
        }
        
        AvailableMana = Mathf.Min(AvailableMana + incomingMana, maxMana);
        manaBar.value = AvailableMana;
        return true;
    }

    private bool _canPlayVo = true;

    public void CheckConditionsForVoiceOver()
    {
        if (_canPlayVo && MathHelper.GetPercent(AvailableMana, maxMana) <= voiceOverData.threshold && MathHelper.CheckChance(voiceOverData.chanceForSaying))
        {
            _canPlayVo = false;
            PlayRandomShot();
        }
    }

    public bool IsPlayerManaFull()
    {
        return AvailableMana == maxMana;
    }

    public void PlayRandomShot()
    {
        if (voiceOverAudioSource.isPlaying) return;
        var randomClip = voiceOverData.audioClips[MathHelper.DrawIndex(0, voiceOverData.audioClips.Count)];
        voiceOverAudioSource.PlayOneShot(randomClip);
    }
}
