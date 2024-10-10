using System;
using System.IO;
using System.Linq;
using UnityEngine;

public class AdminTest : MonoBehaviour
{
  public void CleanImageLoaderEditedPhotos(string path)
  {

    var dir = Path.GetDirectoryName(path);
    var fileName = Path.GetFileNameWithoutExtension(path);
    var imageLoaderCache = "ImageLoaderCache";
    var imageLoaderCacheDir = Path.Combine(dir, imageLoaderCache);
    if (Directory.Exists(imageLoaderCacheDir))
    {
      var files = Directory.GetFiles(imageLoaderCacheDir, $"{fileName}.*", SearchOption.AllDirectories);
      foreach (var file in files)
      {
        File.Delete(file);
        Debug.LogWarning($"ImageLoaderCache Deleted {file}");
      }
    }

  }

  public FitSize GetFitSize(int orgx, int orgy,  int width, int heigth)
  {
    Vector2 orgSize = new Vector2(x: orgx, y: orgy);
    var scale = Math.Min(1.0f, Math.Min(width / orgSize.x, heigth / orgSize.y));
    Vector2 newSize = new Vector2(scale * orgSize.x, scale * orgSize.y);

    return new FitSize((int)Math.Round(newSize.x), (int)Math.Round(newSize.y));
  }
}

public struct FitSize
{
  public int x;
  public int y;

  public FitSize(int x, int y)
  {
    this.x = x;
    this.y = y;
  }
}
