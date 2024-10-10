using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Novena.Cache
{
  /// <summary>
  /// Class designated for caching of Texture2D object.
  /// </summary>
  public class Cache
  {
    /// <summary>
    /// Size of objects in memory. Size in bytes.
    /// </summary>
    public long MemoryOccupied { get; private set; }
    public Dictionary<string, Texture2D> CacheItems { get; }
    public Dictionary<string, Texture2D> NotInCacheItems { get; }

    private readonly CacheManager _cacheManager;

    public Cache(CacheManager cacheManager)
    {
      CacheItems = new Dictionary<string, Texture2D>();
      NotInCacheItems = new Dictionary<string, Texture2D>();
      this._cacheManager = cacheManager;
      UpdateMemoryInfoStatus();
    }

    /// <summary>
    /// Add object in cache
    /// </summary>
    /// <param name="path"></param>
    /// <param name="obj"></param>
    /// <exception cref="CacheException">If error in caching</exception>
    public void Add(string path, Texture2D obj)
    {
      //Lets check do we have item in cache
      if (CacheItems.ContainsKey(path))
      {
        return;
      }

      if (CheckProperties(path, obj) == false)
      {
        Debug.LogError("Missing properties path or obj");
        throw new CacheException("Missing properties path or obj");
      }

      var freeMemorySize = GetFreeMemorySize();
      var currentObjSize = obj.GetRawTextureData().Length;

      if (freeMemorySize > currentObjSize)
      {
        MemoryOccupied += currentObjSize;
        CacheItems.Add(path, obj);
      }
      else
      {
        //Lets clear our cache
        ClearCache();
        freeMemorySize = GetFreeMemorySize();
        //TODO if not added in cache but its loaded in memory we need to handle its destroyment
        if (freeMemorySize > currentObjSize)
        {
          MemoryOccupied += currentObjSize;
          CacheItems.Add(path, obj);
        }
        else
        {
          if (NotInCacheItems.ContainsKey(path) == false)
          {
            NotInCacheItems.Add(path, obj);
          }
        }
      }
      UpdateMemoryInfoStatus();
    }

    /// <summary>
    /// Get object from cache.
    /// </summary>
    /// <param name="path"></param>
    /// <returns>NULL if object not found</returns>
    [CanBeNull]
    public Texture2D Get(string path)
    {
      if (string.IsNullOrEmpty(path))
      {
        throw new CacheException("Missing properties path");
      }

      Texture2D output = null;

      if (CacheItems.ContainsKey(path))
      {
        CacheItems.TryGetValue(path, out output);

        if (output == null)
        {
          CacheItems.Remove(path);
        }
      }

      return output;
    }

    public void ClearCache()
    {
      RawImage[] rawImageComponents = GameObject.FindObjectsOfType(typeof(RawImage), true) as RawImage[];

      List<string> _keysToRemove = new List<string>();
      List<string> _keysToRemoveNotInCache = new List<string>();

      foreach (var item in CacheItems)
      {
        //Check is texture in use.
        if (rawImageComponents.Any((rawImage) => rawImage.texture != null && rawImage.texture.name == item.Value.name) == false)
        {
          //If not then destroy texture and allocate item in dictionary to be removed.
          MemoryOccupied -= item.Value.GetRawTextureData().Length;
          _keysToRemove.Add(item.Key);
          GameObject.Destroy(item.Value);
          continue;
        }
      }

      //Lets handle objects that are in memory but not in cache
      foreach (var item in NotInCacheItems)
      {
        //Check is texture in use.
        if (rawImageComponents.Any((rawImage) => rawImage.texture != null && rawImage.texture.name == item.Value.name) == false)
        {
          //If not then destroy texture and allocate item in dictionary to be removed.
          GameObject.Destroy(item.Value);
          _keysToRemoveNotInCache.Add(item.Key);
          continue;
        }
      }

      foreach (var item in _keysToRemove)
      {
        CacheItems.Remove(item);
      }

      foreach (var item in _keysToRemoveNotInCache)
      {
        NotInCacheItems.Remove(item);
      }
    }

    /// <summary>
    /// Check is props valid
    /// </summary>
    /// <param name="path"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    private bool CheckProperties(string path, Texture2D obj)
    {
      if (string.IsNullOrEmpty(path) || obj == null)
      {
        return false;
      }

      return true;
    }

    private void UpdateMemoryInfoStatus()
    {
      var freeMemorySize = GetFreeMemorySize();

      _cacheManager.FreeMemorySize = (freeMemorySize / 1024f) / 1024f;
      _cacheManager.OccupiedMemorySize = MemoryOccupied / 1024f / 1024f;
      _cacheManager.ItemsInCache = CacheItems.Count;
    }

    /// <summary>
    /// Calculate free memory size of cache.
    /// </summary>
    /// <returns>Size of free memory.</returns>
    private long GetFreeMemorySize()
    {
      var cacheTotalSize = _cacheManager.TotalMemorySize * 1024 * 1024;

      if (cacheTotalSize > MemoryOccupied)
      {
        var size = cacheTotalSize - MemoryOccupied;
        return size;
      }

      return 0;
    }
  }
}