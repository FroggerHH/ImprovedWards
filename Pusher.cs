using System;
using Extensions;
using UnityEngine;

namespace ImprovedWards;

public class Pusher : MonoBehaviour
{
    internal PrivateArea ward;

    private void Awake() => ward = GetComponentInParent<PrivateArea>();

    private void OnTriggerExit(Collider other)
    {
        if (!ward || !ward.m_nview.IsOwner()) return;
        var character = other.GetComponent<Character>();
        if (!character) return;
        var data = character.GetAdditionalData();
        if (!data.collider) data.Init(character);

        data.rigidbody.velocity = Vector3.zero;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!ward || !ward.m_nview.IsOwner()) return;
        var character = other.GetComponent<Character>();
        if (!character) return;
        
        if (character.IsPlayer() && PrivateArea.CheckAccess(other.transform.position, flash: false)) return;
        
        var data = character.GetAdditionalData();
        if (!data.collider) data.Init(character);

        var dir = (other.transform.position - transform.position).normalized;
        dir.y = 0;
        data.rigidbody.velocity = dir;
    }
}