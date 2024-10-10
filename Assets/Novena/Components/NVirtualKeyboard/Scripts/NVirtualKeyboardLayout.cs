using UnityEngine;

namespace Novena.VirtualKeyboard
{
    public class NVirtualKeyboardLayout : MonoBehaviour
    {
        public NVirtualKeyboardLayoutType LayoutType = NVirtualKeyboardLayoutType.NORMAL;
    }

    public enum NVirtualKeyboardLayoutType
    {
        NORMAL,
        NUMBER
    }
}
