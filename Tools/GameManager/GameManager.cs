using UnityEngine;

public class GameManager : MonoBehaviour
{
    // singleton example
    private static GameManager _instance;
    public static GameManager Instance => _instance;
    private void Awake()
    {
        // check for duplicate singletons
        if (_instance != null)
        {
            Debug.LogWarning($"Duplicate singleton found! {gameObject.name}");
            Destroy(gameObject);
        }
        _instance = this;
    }
}