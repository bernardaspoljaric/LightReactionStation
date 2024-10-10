using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Novena.Cache;
using Cysharp.Threading.Tasks;
using Novena.DAL;
using Novena.Networking.Image;
using Novena.UiUtility.Base;
using UnityEngine;
using UnityEngine.UI;

public class PhotoLoadingEditing : UiController
{
  [SerializeField] private RawImage _rawImage1Org;
  [SerializeField] private RawImage _rawImage1Fill;
  [SerializeField] private RawImage _rawImage1FillBase2;
  [Space(10)]
  [SerializeField] private RawImage _rawImage2Org;
  [SerializeField] private RawImage _rawImage2Fill;
  [SerializeField] private RawImage _rawImage2FillBase2;
  [Space(10)]
  [SerializeField] private RawImage _rawImage3Org;
  [SerializeField] private RawImage _rawImage3Fill;
  [SerializeField] private RawImage _rawImage3FillBase2;
  public override void OnShowViewStart()
  {
    LoadPhotos().Forget();
    base.OnShowViewFinished();
  }

  async UniTaskVoid LoadPhotos ()
  {
    var photos = Data.TranslatedContent.GetThemeByName("Gallery Demo").GetMediaByName("Gallery").Photos;

    for (int i = 0; i < photos.Length; i++)
    {
      var photo = photos[i];

      switch (i)
      {
        case 0:
          await _rawImage1Org.LoadImageAsync(photo.FullPath);

          await _rawImage1Fill.LoadImageAsync(photo.FullPath, options: new ImageLoader.Options(color: Color.white, scaleType: ImageLoader.ScaleType.Fill, width: 260, height: 260));

          await _rawImage1FillBase2.LoadImageAsync(photo.FullPath, options: new ImageLoader.Options(color: new Color(1, 1, 1, 0), scaleType: ImageLoader.ScaleType.Fill, width: 250, height: 250));

          break;
        case 1:
          await _rawImage2Org.LoadImageAsync(photo.FullPath);
          await _rawImage2Fill.LoadImageAsync(photo.FullPath, options: new ImageLoader.Options(color: Color.white, scaleType: ImageLoader.ScaleType.Fill, width: 500, height: 500));
          await _rawImage2FillBase2.LoadImageAsync(photo.FullPath, options: new ImageLoader.Options(color: new Color(1, 1, 1, 0), scaleType: ImageLoader.ScaleType.Fill, width: 250, height: 250));
          break;
        case 2:
          await _rawImage3Org.LoadImageAsync(photo.FullPath);
          await _rawImage3Fill.LoadImageAsync(photo.FullPath, options: new ImageLoader.Options(color: new Color(1, 1, 1, 0), scaleType: ImageLoader.ScaleType.Fill, width: 260, height: 260));

          await _rawImage3FillBase2.LoadImageAsync(photo.FullPath, options: new ImageLoader.Options(color: new Color(1, 1, 1, 0), scaleType: ImageLoader.ScaleType.Crop, width: 700, height: 700, leftOffset: 100, topOffset: 100));
          break;
        default:
          break;
      }
    }    
  }
}
