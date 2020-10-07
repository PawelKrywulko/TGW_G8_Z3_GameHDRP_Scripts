using UnityEngine;

public class RagdollPlayerController : RagdollBase
{
    private PlayerMovement _playerMovement;
    private CastSpell _castSpell;
    private Dash _dash;
    private Stab _stab;

    protected override void InitCustomComponents()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _castSpell = GetComponent<CastSpell>();
        _dash = GetComponent<Dash>();
        _stab = GetComponent<Stab>();
    }

    protected override void DisableCustomComponents()
    {
        _playerMovement.enabled = false;
        _castSpell.enabled = false;
        _dash.enabled = false;
        _stab.enabled = false;
    }

    private void EnableCustomComponents()
    {
        _playerMovement.enabled = true;
        _castSpell.enabled = true;
        _dash.enabled = true;
        GetComponent<CapsuleCollider>().enabled = true;
        GetComponent<Animator>().enabled = true;
        transform.position = new Vector3(transform.position.x, 20, transform.position.z);
    }

    public void DisableRagdollEffect()
    {
        EnableCustomComponents();
        SwitchRagdollComponents();
    }

    public void SwitchCustomComponents()
    {
        _playerMovement.enabled = !_playerMovement.enabled;
        _castSpell.enabled = !_castSpell.enabled;
        _dash.enabled = !_dash.enabled;
        _stab.enabled = !_stab.enabled;
    }
}