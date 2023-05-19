using Unity.Netcode;
using UnityEngine;

public class CharacterNetworkManager : NetworkBehaviour
{
    [Header("Position")] 
    public NetworkVariable<Vector3> NetworkPosition = new NetworkVariable<Vector3>(Vector3.zero,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<Quaternion> NetworkRotation = new NetworkVariable<Quaternion>(Quaternion.identity,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public Vector3 networkPositiveVelocity;
    public float networkPositionSmoothTime = 0.1f;
    public float networkRotationSmoothTime = 0.1f;
}
