using System.Collections;
using Mirror;
using Network;
using Tyrants;
using UnityEngine;

[RequireComponent(typeof(TyrantMoveLogic))]
public class EyeTyrant : BaseTyrant
{
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float forceAttackRadius;
    [SyncVar(hook = nameof(PlaySound))] private bool _isAttacking;

    private TyrantMoveLogic _moveLogic;

    private void Awake()
    {
        _moveLogic = GetComponent<TyrantMoveLogic>();
    }

    private void Update()
    {
        if (!isServer || _isAttacking) return;

        if (!_moveLogic.FindTarget(out NetworkIdentity target, forceAttackRadius, playerMask, QueryTriggerInteraction.Ignore)) return;
        CmdSetSelectedUser(target);
        CmdStartAttacking();
    }

    [Command(requiresAuthority = false)]
    private void CmdStartAttacking()
    {
        _isAttacking = true;
        StartCoroutine(WaitBeforeMove());
    }

    private void PlaySound(bool oldValue, bool newValue)
    {
        audioSource.Play();
    }

    IEnumerator WaitBeforeMove() // Server only
    {
        if (!isServer) yield break;
        yield return new WaitForSeconds(5.5f);

        if (lastSelected == null) yield break;
        if (!lastSelected.TryGetComponent(out LocalPlayer player)) yield break;
        if (player.IsHide)
        {
            Debug.Log($"Target player: {lastSelected.netId} is hide");
            yield break;
        }

        StartCoroutine(_moveLogic.MoveToTarget(player.transform, 10, (() => player.CmdKillPlayer())));
    }

    public override void OnSelect(NetworkIdentity networkIdentity)
    {
        base.OnSelect(networkIdentity);
        if (_isAttacking) return;
        CmdStartAttacking();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, forceAttackRadius);
    }
}