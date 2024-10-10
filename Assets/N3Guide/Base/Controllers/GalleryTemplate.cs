using Cysharp.Threading.Tasks;
using Novena.DAL;
using Novena.Networking.Image;
using Novena.UiUtility.Base;
using Scripts.Gallery;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.N3Guide.Base.Controllers
{
  public class GalleryTemplate : UiController
  {
    [Header("Components")] 
    [SerializeField] private Gallery _gallery;
    [SerializeField] private RawImage _testRawImage;
    

    public override void OnShowViewStart()
    {
      LoadTestImage().Forget();
      SetupGallery();
    }

    private void SetupGallery()
    {
      _gallery.Setup(Data.Theme);
    }

    private async UniTaskVoid LoadTestImage()
    {
      var path = "https://novena.hr/UserFiles/Image/novosti/webvijesti/festival%20dizajna%20zgdw/zgdw.jpg";
      var texture = await _testRawImage.LoadImageAsync(path);
    }
  }
}