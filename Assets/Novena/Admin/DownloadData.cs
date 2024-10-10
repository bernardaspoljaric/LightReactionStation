using System.Collections.Generic;
using UnityEngine;

namespace Novena.Admin
{
  public class DownloadData : MonoBehaviour
  {
    public static DownloadData Instance { get; private set; }

    [Header("Download codes of guides to download")]
    public List<DownloadCode> DownloadCodes = new List<DownloadCode>();

    private void Awake()
    {
      Instance = this;
    }
  }

  [System.Serializable]
  public class DownloadCode
  {
    public string Code;
    public string Name;
  }
}
