using MyObjectPooler;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider))]
public class KillZone : CustomEventTrigger
{
    public void Kill(GameObject go)
    {
        if (go.TryGetComponent(out PooledObject pooled))
        {
            pooled.ReturnToPool();
            return;
        }


        if (go.CompareTag("Player"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("DeathMenu");
             
        }


        /*if (go.TryGetComponent(out PlayerHealth playerHealth))
        {
            DamageInfo infoPlayer = new DamageInfo(50000f, false, go, gameObject, 1f, 1f, 1f);
            playerHealth.ForceTakeDamage(infoPlayer);
            return;
        }
        
        if (go.TryGetComponent(out ProjectileBase projectile))
        {
            projectile.CleanUp();
            return;
        }
        ProjectileBase projectileParent = go.GetComponentInParent<ProjectileBase>();
        if (projectileParent != null)
        {
            projectileParent.CleanUp();
            return;
        }
        
        if (!go.TryGetComponent(out IDamageable damageable))
        {
            Destroy(go);
            return;
        }
        
        
        DamageInfo info = new DamageInfo(50000f, false, go, gameObject, 1f, 1f, 1f);
        damageable.TakeDamage(info);
        */
    }
}
