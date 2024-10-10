using Novena.Kiosk;
using UnityEngine;

namespace Novena
{
    public class NKioskController : MonoBehaviour
    {
        private Canvas _canvas;
        private NKiosk _kiosk;
        // Start is called before the first frame update
        void Start()
        {
            _canvas = GetComponent<Canvas>();
            _kiosk = GetComponent<NKiosk>();
            Hide();
        }

        public void Show()
        {
            _canvas.enabled = true;
            _kiosk.Initialize();
        }

        public void Hide()
        {
            _canvas.enabled = false;
        }     
    }
}
