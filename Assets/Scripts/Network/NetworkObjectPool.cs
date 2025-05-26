using Fusion;
using SkillBoxFinal;
using System.Collections.Generic;
using UnityEngine;

public class NetworkObjectPool : NetworkBehaviour, INetworkObjectPool
{
    private Dictionary<GameObject, Queue<NetworkObject>> _pooledObjects = new();


   /* private NetworkObject CreateNewObject()
    {
        NetworkObject obj = Runner.Spawn(
            _prefab,
            Vector3.zero,
            Quaternion.identity,
            PlayerRef.None
        );
        obj.gameObject.SetActive(false);
        _pooledObjects.Enqueue(obj);
        return obj;
    }*/

    public NetworkObject GetNetworkObject(GameObject prefab)
    {
        if (!_pooledObjects.ContainsKey(prefab) || _pooledObjects[prefab].Count == 0)
            return null;

        NetworkObject obj = _pooledObjects[prefab].Dequeue();
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void ReturnNetworkObject(GameObject prefab, NetworkObject obj)
    {
        if (!_pooledObjects.ContainsKey(prefab))
            _pooledObjects[prefab] = new Queue<NetworkObject>();
        obj.gameObject.SetActive(false);
        _pooledObjects[prefab].Enqueue(obj);
    }
}