using UnityEngine;

public class StepSoundWrapper : MonoBehaviour
{
    [SerializeField] private FootStepsSoundManager footStepsSoundManager = default;
    [SerializeField] private WoodStepsSoundManager woodStepsSoundManager = default;
    [SerializeField] private LayerMask woodLayerMask = default;

    private bool _isTouchingWood;

    private void Update()
    {
        _isTouchingWood = Physics.OverlapSphere(transform.position, 1f, woodLayerMask).Length > 0;
    }

    private void PlayStepSound() //Invoked as Animation Event
    {
        if (_isTouchingWood)
        {
            woodStepsSoundManager.PlayClipWithRandomVolumeAndPitch();
        }
        else
        {
            footStepsSoundManager.PlayClipWithRandomVolumeAndPitch();
        }
    }
}