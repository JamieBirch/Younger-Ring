using System;
using UnityEngine;
using Unity.Netcode;

public class CharacterManager : NetworkBehaviour
{
    public CharacterController CharacterController;

    private CharacterNetworkManager _characterNetworkManager;
    protected virtual void Awake()
    {
        DontDestroyOnLoad(this);

        CharacterController = GetComponent<CharacterController>();
        _characterNetworkManager = GetComponent<CharacterNetworkManager>();
    }

    protected virtual void Update()
    {
        if (IsOwner)
        {
            _characterNetworkManager.NetworkPosition.Value = transform.position;
            _characterNetworkManager.NetworkRotation.Value = transform.rotation;
        }
        else
        {
            transform.position = Vector3.SmoothDamp(
                transform.position, 
                _characterNetworkManager.NetworkPosition.Value,
                ref _characterNetworkManager.networkPositiveVelocity, 
                _characterNetworkManager.networkPositionSmoothTime
                );

            transform.rotation = Quaternion.Slerp(transform.rotation, _characterNetworkManager.NetworkRotation.Value,
                _characterNetworkManager.networkRotationSmoothTime);
        }
    }
}
