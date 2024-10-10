using Cysharp.Threading.Tasks;
using Novena.DAL;
using Novena.Networking.Image;
using Novena.UiUtility.Base;
using UnityEngine;
using UnityEngine.UI;

public class ImageLoaderExampleController : UiController
{
  [Header("RawImage Components")]
  [SerializeField] private RawImage rawImageEnvelope;
  [SerializeField] private RawImage rawImageEnvelopeToNew;
  [SerializeField] private RawImage rawImageFit;
  [SerializeField] private RawImage rawImageFitFill;
  [SerializeField] private RawImage rawImageCrop;
  [SerializeField] private RawImage rawImageScale;
  [SerializeField] private RawImage rawImageOriginal;
  public override void OnShowViewFinished() {
    LoadImages().Forget();
    base.OnShowViewStart(); 
  }

  private async UniTaskVoid LoadImages()
  {
    try
    {
      var photos = Data.TranslatedContent.GetThemeByName("Gallery Demo").GetMediaByName("Gallery").Photos;
      var photo = photos[0];

      await rawImageEnvelope.LoadImageAsync(photo.FullPath, new ImageLoader.Options(color: new Color(1,1,1,0), scaleType: ImageLoader.ScaleType.Envelope, width: 190, height: 140));

      await rawImageEnvelopeToNew.LoadImageAsync(photo.FullPath, new ImageLoader.Options(color: new Color(1, 1, 1, 0), scaleType: ImageLoader.ScaleType.EnvelopeToNew, width: 190, height: 140));

      await rawImageFit.LoadImageAsync(photo.FullPath, new ImageLoader.Options(color: new Color(1, 1, 1, 0), scaleType: ImageLoader.ScaleType.Fit, width: 190, height: 140));

      await rawImageFitFill.LoadImageAsync(photo.FullPath, new ImageLoader.Options(color: Color.grey, scaleType: ImageLoader.ScaleType.Fill, width: 190, height: 140));

      await rawImageCrop.LoadImageAsync(photo.FullPath, new ImageLoader.Options(color: new Color(1, 1, 1, 0), scaleType: ImageLoader.ScaleType.Crop, width: 190, height: 140, leftOffset: 100, topOffset: 500));

      await rawImageScale.LoadImageAsync(photo.FullPath, new ImageLoader.Options(color: new Color(1, 1, 1, 0), scaleType: ImageLoader.ScaleType.Scale, width: 190, height: 140));

      await rawImageOriginal.LoadImageAsync(photo.FullPath);
    }
    catch (System.Exception e)
    {
      Debug.LogException(e);
    }
  }
}
