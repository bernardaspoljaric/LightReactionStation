using UnityEngine;

namespace Novena.Components.ProgressIndicator
{
  public class InfiniteProgressIndicator : MonoBehaviour
  {
    [SerializeField] private GameObject _imageIndicator;

    private void Awake()
    {
      gameObject.SetActive(false);
    }

    public void Show()
    {
      gameObject.SetActive(true);
    }

    public void Hide()
    {
      gameObject.SetActive(false);
    }

    private void Update()
    {
      var vector = new Vector3(0, 0, -1);
      _imageIndicator.transform.Rotate(vector * 100 * Time.deltaTime);
    }
  }
}