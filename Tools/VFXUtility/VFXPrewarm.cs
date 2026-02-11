using MyObjectPooler;
using System.Collections;
using UnityEngine;

public class VFXPrewarm : MonoBehaviour
{
    [SerializeField] private PooledObject[] _vfxPrefabs;
    [SerializeField, Min(1)] private int _instancesPerPrefab = 2;
    [SerializeField] private float _betweenSpawnDelay = 0.01f;
    [SerializeField] private bool _log = false;


    private void Start()
    {
        StartCoroutine(Prewarm());
    }
    private IEnumerator Prewarm()
    {

        yield return null;

        if (_vfxPrefabs == null || _vfxPrefabs.Length == 0)
            yield break;
        
        foreach (var prefab in _vfxPrefabs)
        {
            if (prefab == null)
                continue;

            int count = Mathf.Clamp(_instancesPerPrefab, 1, prefab.PoolSize);
            PooledObject[] pooled = new PooledObject[count];
            for (int i = 0; i < count; i++)
            {
                pooled[i] = PoolSystem.Instance.Get(prefab, Vector3.zero, Quaternion.identity);

                //wait 0.1f for vfx to take effect
                yield return new WaitForSeconds(0.1f);
                if (_betweenSpawnDelay > 0f)
                    yield return new WaitForSeconds(_betweenSpawnDelay);
            }
            foreach (var pooledObj in pooled)
            {
                if (pooled == null) continue;
                if(pooledObj == null) continue;
                if(pooledObj.IsInPool) continue;
                pooledObj.ReturnToPool();
            }
        }
    }
}
