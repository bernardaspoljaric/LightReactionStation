#nullable enable
using System;
using System.IO;
using Assets.Novena.Cache;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Novena.Networking.Image
{
  /// <summary>
  /// Image utility for downloading and loading of images.
  /// </summary>
  public static class ImageLoader
  {
    public struct Options
    {
      public readonly int width;
      public readonly int height;
      public readonly int leftOffset;
      public readonly int topOffset;
      public readonly Color fillColor;
      public readonly ScaleType scaleType;
      public readonly TextureFormat textureFormat;
      public readonly bool setAspectRatio;

      public Options(Color color, int width = -1, int height = -1, int leftOffset = 0, int topOffset = 0, ScaleType scaleType = ScaleType.None, TextureFormat textureFormat = TextureFormat.RGBA32, bool setAspectRatio = true)
      {
        this.width = width;
        this.height = height;
        this.leftOffset = leftOffset;
        this.topOffset = topOffset;
        this.fillColor = color;
        this.scaleType = scaleType;
        this.textureFormat = textureFormat;
        this.setAspectRatio = setAspectRatio;
      }
    }

    /// <summary>
    /// Types of image scalings.
    /// <para>Scale - Downscale only.</para>
    /// <para>Crop - Crop image to dimensions and offsets.</para>
    /// <para>Fill - Scale image to dimensions and fill rest of space with color.</para>
    /// <para>Fit - Scale image to fit in parent.</para>
    /// <para>Envelope - Use same image as Fit but it will be enveloped.</para>
    /// <para>EnvelopeToNew - Creates new cached file with calculated envelope dimensions.</para>
    /// </summary>
    public enum ScaleType { None, Scale, Crop, Fill, Fit, Envelope, EnvelopeToNew }

    /// <summary>
    /// Image Scale to fit
    /// </summary>
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

    /// <summary>
    /// Download image from URI.
    /// </summary>
    /// <param name="path"></param>
    /// <returns>Texture2D if successful, NULL if error in download</returns>
    public static async UniTask<Texture2D?> GetTexture(string path)
    {
      try
      {
        var response = await UnityWebRequestTexture.GetTexture(path, true).SendWebRequest();

        if (response.result != UnityWebRequest.Result.Success)
        {
          Debug.LogError("ImageLoader GetTexture: " + response.error);
        }

        if (response.result == UnityWebRequest.Result.Success)
        {
          return DownloadHandlerTexture.GetContent(response);
        }
      }
      catch (UnityWebRequestException e)
      {
        Debug.LogError(e);
        return null;
      }

      return null;
    }

    /// <summary>
    /// Download image and set it to rawImage with calculated aspect ratio if aspect ratio fitter exist.
    /// </summary>
    /// <param name="path">File path or Url</param>
    /// <param name="rawImage">RawImage component to apply texture</param>
    /// <example>
    /// <code>ImageLoader.LoadImageAsync("imagePath/file.png", rawImage)</code>
    /// </example>
    [Obsolete("Not in use anymore! Use")]
    public static async UniTask<bool> LoadImageAsync(string path, RawImage rawImage)
    {
      bool output = false;

      var texture = await GetTexture(path);

      if (texture != null)
      {
        try
        {
          texture.name = path;
          rawImage.texture = texture;
          SetAspectRatio(texture.width, texture.height, rawImage);
          output = true;
        }
        catch (Exception e)
        {
          Console.WriteLine(e);
        }
      }

      return output;
    }

    /// <summary>
    /// Load image Async.
    /// </summary>
    /// <param name="path">Path to file</param>
    /// <returns>Texture2D or NULL if nothing loaded.</returns>
    public static async UniTask<Texture2D?> LoadImageAsync(string path)
    {
      return await Load(null, path, new Options(new Color(1, 1, 1, 0)));
    }

    /// <summary>
    /// Load image Async.
    /// Stores in cache.
    /// </summary>
    /// <param name="rawImage">Where to apply texture.</param>
    /// <param name="path">Path to image</param>
    /// <returns>Texture2D or NULL if nothing loaded.</returns>
    /// <description>rawImage.LoadImageAsync("imagePath/file.png")</description>
    /// <example>
    /// <code>rawImage.LoadImageAsync("imagePath/file.png")</code>
    /// </example>
    public static async UniTask<Texture2D?> LoadImageAsync(this RawImage rawImage, string path)
    {
      return await Load(rawImage, path, new Options(Color.white, setAspectRatio: true));
    }

    /// <summary>
    /// Load image Async.
    /// Stores in cache.
    /// </summary>
    /// <param name="rawImage"></param>
    /// <param name="path"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static async UniTask<Texture2D?> LoadImageAsync(this RawImage rawImage, string path, Options options = new Options())
    {
      return await Load(rawImage, path, options);
    }

    /// <summary>
    /// Load image to RawImage.
    /// </summary>
    /// <param name="rawImage"></param>
    /// <param name="path"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    private static async UniTask<Texture2D?> Load(RawImage? rawImage, string path, Options options)
    {
      Texture2D? output = null;

      var dir = Path.GetDirectoryName(path);
      var fileName = Path.GetFileName(path);
      string newDirectory = "";

      switch (options.scaleType)
      {
        case ScaleType.None:
          break;
        case ScaleType.Scale:
          newDirectory = $"ImageLoaderCache\\scale\\{options.width}x{options.height}\\";
          break;
        case ScaleType.Fit or ScaleType.Envelope:
          newDirectory = $"ImageLoaderCache\\fit\\{options.width}x{options.height}\\";
          break;
        case ScaleType.EnvelopeToNew:
          newDirectory = $"ImageLoaderCache\\envelope\\{options.width}x{options.height}\\";
          break;
        case ScaleType.Crop:
          newDirectory = $"ImageLoaderCache\\crop\\{options.leftOffset}_{options.width}x{options.topOffset}_{options.height}\\";
          break;
        case ScaleType.Fill:
          newDirectory = $"ImageLoaderCache\\fill\\{GetDirectoryNameFromColor(options.fillColor)}\\{options.width}x{options.height}\\";
          fileName = Path.ChangeExtension(fileName, ".png");
          break;
        default:
          break;
      }

      string newPath = Path.Combine(dir, newDirectory, fileName);

      var cacheTexture = CacheManager.Instance.Cache.Get(newPath);

      if (cacheTexture == null)
      {
        try
        {
          if (File.Exists(newPath))
          {
            output = await TextureOps.LoadImageAsync(newPath);
            output.name = newPath;
            CacheManager.Instance.Cache.Add(newPath, output);
          }
          else
          {
            var texture = await TextureOps.LoadImageAsync(path);

            Texture2D editedTexture = new Texture2D(1, 1);

            switch (options.scaleType)
            {
              case ScaleType.None:
                break;
              case ScaleType.Scale:
                editedTexture = TextureOps.Scale(texture, options.width, options.height);
                break;
              case ScaleType.Fit or ScaleType.Envelope:
                var fitSize = GetFitSize(texture, options.width, options.height);
                editedTexture = TextureOps.Scale(texture, fitSize.x, fitSize.y);
                break;
              case ScaleType.EnvelopeToNew:
                var enevelopeSize = GetFitSize(texture, options.width, options.height, true);
                editedTexture = TextureOps.Scale(texture, enevelopeSize.x, enevelopeSize.y);
                break;
              case ScaleType.Crop:
                editedTexture = TextureOps.Crop(texture, options.leftOffset, options.topOffset, options.width, options.height);
                break;
              case ScaleType.Fill:
                editedTexture = TextureOps.ScaleFill(texture, options.width, options.height, options.fillColor);
                break;

              default:
                break;
            }

            CacheManager.Instance.Cache.Add(newPath, editedTexture);
            TextureOps.SaveImage(editedTexture, newPath);
            GameObject.DestroyImmediate(texture);
            output = editedTexture;
            output.name = newPath;
          }
        }
        catch (Exception e)
        {
          Debug.LogException(e);
        }
      }
      else
      {
        output = cacheTexture;
      }

      if (output != null && rawImage != null)
      {
        rawImage.texture = output;

        if (options.scaleType == ScaleType.Envelope || options.scaleType == ScaleType.EnvelopeToNew)
        {
          SetAspectRatio(output.width, output.height, rawImage, AspectRatioFitter.AspectMode.EnvelopeParent);
        }
        else
        {
          if (options.setAspectRatio)
          {
            SetAspectRatio(output.width, output.height, rawImage);
          }
        }
      }

      return output;
    }

    private static string GetDirectoryNameFromColor(Color color)
    {
      return $"{color.r}{color.g}{color.b}{color.a}";
    }

    /// <summary>
    /// Sets aspect ratio on AspectRatioFitter of RawImage.
    /// </summary>
    /// <param name="texture"></param>
    /// <param name="rawImage"></param>
    /// <exception cref="Exception">If no AspectRatioFitter component is attached.</exception>
    public static void SetAspectRatio(int width, int height, RawImage rawImage, AspectRatioFitter.AspectMode aspectMode = AspectRatioFitter.AspectMode.FitInParent)
    {
      float aspectRatio = (float)width / (float)height;

      try
      {
        var aspectRatioFitter = rawImage.GetComponent<AspectRatioFitter>();
        aspectRatioFitter.aspectRatio = aspectRatio;
        aspectRatioFitter.aspectMode = aspectMode;
      }
      catch (Exception e)
      {
        Debug.Log(e.Message);
      }
    }

    /// <summary>
    /// Calculates texture size to fit in dimensions.
    /// </summary>
    /// <param name="originalTexture"></param>
    /// <param name="width"></param>
    /// <param name="heigt"></param>
    /// <param name="isEnvelope">Set to true if envelope calculation needed</param>
    /// <returns></returns>
    private static FitSize GetFitSize(Texture2D originalTexture, int width, int heigth, bool isEnvelope = false)
    {
      Vector2 orgSize = new Vector2(x: originalTexture.width, y: originalTexture.height);
      float scale = 0;
      if (isEnvelope)
      {
        //Calculate to envelope
        scale = Math.Min(1.0f, Math.Max(width / orgSize.x, heigth / orgSize.y));
      }
      else
      {
        //Calculate to fit
        scale = Math.Min(1.0f, Math.Min(width / orgSize.x, heigth / orgSize.y));
      }

      Vector2 newSize = new Vector2(scale * orgSize.x, scale * orgSize.y);

      return new FitSize((int)Math.Round(newSize.x), (int)Math.Round(newSize.y));
    }
  }
}