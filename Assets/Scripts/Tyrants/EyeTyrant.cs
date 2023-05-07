using System.Collections;
using Mirror;
using Network;
using Tyrants;
using UnityEngine;

[RequireComponent(typeof(TyrantMoveLogic))]
public class EyeTyrant : BaseTyrant
{
    [SerializeField] private AudioSource audioSource;
    private TyrantMoveLogic _moveLogic;

    private void Awake()
    {
        _moveLogic = GetComponent<TyrantMoveLogic>();
        OnAttackingChanged.AddListener(PlayClip);
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        OnAttackingChanged.AddListener(PlayClip);
    }

    private void Update()
    {
        if (!isServer || IsAttacking) return;

        if (!_moveLogic.FindClosestIdentityInRange(out NetworkIdentity target, findRadius, targetMask, QueryTriggerInteraction.Ignore)) return;
        CmdSetSelectedUser(target);
        CmdStartAttacking();
    }

    [Command(requiresAuthority = false)]
    private void CmdStartAttacking()
    {
        IsAttacking = true;
        StartCoroutine(WaitBeforeMove());
    }

    private void PlayClip(bool newValue)
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
        if (IsAttacking) return;
        CmdStartAttacking();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, findRadius);
    }
}