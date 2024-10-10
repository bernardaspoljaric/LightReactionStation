using UnityEngine;

namespace Assets.Novena.Cache
{
  public class CacheManager : MonoBehaviour
  {
    [Header("Settings")]
    [Tooltip("How much memory to occupy in MB")]
    [SerializeField] public int TotalMemorySize = 200;
    [Space(10)]
    [Header("Memory info")]
    [SerializeField] public float OccupiedMemorySize = 0;
    [SerializeField] public float FreeMemorySize = 0;
    [SerializeField] public int ItemsInCache = 0;

    /// <summary>
    /// Singleton
    /// </summary>
    public static CacheManager Instance { get; private set; }
    public Cache Cache { get; private set; }

    public void Awake()
    {
      Init();
    }

    private void Init()
    {
      if (Instance == null)
      {
        Instance = this;
      }

      if (Cache == null)
      {
        Cache = new Cache(this);
      }
    }
    public void ClearCache()
    {
      Cache.ClearCache();
    }
  }
}
