using UnityEngine;
using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections;

public class RawInputHandler : MonoBehaviour
{
    private const int WM_INPUT = 0x00FF;
    private const int RIM_TYPEKEYBOARD = 1;
    private const int RID_INPUT = 0x10000003;
    private const int RID_HEADER = 0x10000005;
    private const uint RIDI_DEVICENAME = 0x20000007;

    [StructLayout(LayoutKind.Sequential)]
    struct RAWINPUTDEVICE
    {
        public ushort usUsagePage;
        public ushort usUsage;
        public uint dwFlags;
        public IntPtr hwndTarget;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct RAWINPUTHEADER
    {
        public int dwType;
        public int dwSize;
        public IntPtr hDevice;
        public IntPtr wParam;
    }

    [StructLayout(LayoutKind.Explicit)]
    struct RAWINPUT
    {
        [FieldOffset(0)]
        public RAWINPUTHEADER header;

        [FieldOffset(16)]
        public RAWKEYBOARD keyboard;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct RAWKEYBOARD
    {
        public ushort MakeCode;
        public ushort Flags;
        public ushort Reserved;
        public ushort VKey;
        public uint Message;
        public uint ExtraInformation;
    }

    [DllImport("user32.dll")]
    static extern uint GetRawInputData(
        IntPtr hRawInput,
        uint uiCommand,
        IntPtr pData,
        ref uint pcbSize,
        uint cbSizeHeader
    );

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool RegisterRawInputDevices(
        RAWINPUTDEVICE[] pRawInputDevices,
        uint uiNumDevices,
        uint cbSize
    );

    [DllImport("user32.dll", SetLastError = true)]
    static extern uint GetRawInputDeviceInfo(
        IntPtr hDevice,
        uint uiCommand,
        IntPtr pData,
        ref uint pcbSize
    );

    [DllImport("user32.dll", SetLastError = true)]
    static extern uint GetRawInputDeviceInfo(
        IntPtr hDevice,
        uint uiCommand,
        [Out] StringBuilder pData,
        ref uint pcbSize
    );

    [DllImport("user32.dll")]
    static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    static extern bool PeekMessage(
        out MSG lpMsg,
        IntPtr hWnd,
        uint wMsgFilterMin,
        uint wMsgFilterMax,
        uint wRemoveMsg
    );

    [DllImport("user32.dll")]
    static extern bool TranslateMessage(ref MSG lpMsg);

    [DllImport("user32.dll")]
    static extern IntPtr DispatchMessage(ref MSG lpMsg);

    [DllImport("user32.dll")]
    public static extern int ToUnicode(
        uint wVirtKey,
        uint wScanCode,
        byte[] lpKeyState,
        [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pwszBuff,
        int cchBuff,
        uint wFlags
    );

    [StructLayout(LayoutKind.Sequential)]
    public struct MSG
    {
        public IntPtr hwnd;
        public uint message;
        public IntPtr wParam;
        public IntPtr lParam;
        public uint time;
        public POINT pt;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int x;
        public int y;
    }

    private IntPtr windowHandle;

    private void Start()
    {
        windowHandle = GetActiveWindow();

        RAWINPUTDEVICE[] rid = new RAWINPUTDEVICE[1];
        rid[0].usUsagePage = 0x01;
        rid[0].usUsage = 0x06; // Keyboard
        rid[0].dwFlags = 0;
        rid[0].hwndTarget = windowHandle;

        if (!RegisterRawInputDevices(rid, (uint)rid.Length, (uint)Marshal.SizeOf(rid[0])))
        {
            Debug.LogError("Failed to register raw input device(s).");
            throw new ApplicationException("Failed to register raw input device(s).");
        }
        else
        {
            Debug.Log("Successfully registered raw input device(s).");
        }
    }

    ulong guiFrameCount = 0;

    private void OnGUI()
    {
        guiFrameCount++;

       /*  if (Input.anyKey)
        {
            if (Input.inputString != "")
            {
                Debug.Log(
                    $"RawInput ONGUI - {Input.inputString} frame: {Time.frameCount} gui: {guiFrameCount}"
                );
            }
            ProcessMessages(Input.inputString, guiFrameCount);
        } */
    }

    private void Update()
    {
        if (Input.anyKey)
        {
            if (Input.inputString != "")
            {
                Debug.Log($"Update - {Input.inputString} frame: {Time.frameCount}");
                StartCoroutine(GetDeviceNameCoroutine(Input.inputString));
            }
        }
    }

    private IEnumerator GetDeviceNameCoroutine(string inputString)
    {
        bool messageReceived = true;
        int count = 0;

        Debug.Log(
                    $"START OF SEARCH FOR DEVICE. frame: {Time.frameCount} gui: {guiFrameCount}"
                );

        while (messageReceived)
        {
            if (count > 120)
            {
                Debug.Log(
                    $"END OF SEARCH FOR DEVICE. frame: {Time.frameCount} gui: {guiFrameCount}"
                );
                break;
            }

            for (int i = 0; i < 5; i++)
            {
                while (PeekMessage(out var msg, IntPtr.Zero, 0, 0, 1))
                {
                    if (msg.message == WM_INPUT)
                    {
                        messageReceived = false;
                        Debug.Log(
                            $"GetDeviceNameCoroutine message received. frame: {Time.frameCount} gui: {guiFrameCount}"
                        );
                        ProcessRawInput(msg.lParam, inputString, guiFrameCount);
                    }

                    TranslateMessage(ref msg);
                    DispatchMessage(ref msg);
                }
                
                if (!messageReceived)
                {
                    break;
                }
            }

            count++;
            yield return null;
        }
    }

    private void ProcessMessages(string inputString, ulong guiFrameCount)
    {
        while (PeekMessage(out var msg, IntPtr.Zero, 0, 0, 1))
        {
            if (msg.message == WM_INPUT)
            {
                Debug.Log(
                    $"WM_INPUT message received. frame: {Time.frameCount} gui: {guiFrameCount}"
                );
                ProcessRawInput(msg.lParam, inputString, guiFrameCount);
            }

            TranslateMessage(ref msg);
            DispatchMessage(ref msg);
        }
    }

    private void ProcessRawInput(IntPtr hRawInput, string inputString, ulong guiFrameCount)
    {
        uint dwSize = 0;
        GetRawInputData(
            hRawInput,
            RID_INPUT,
            IntPtr.Zero,
            ref dwSize,
            (uint)Marshal.SizeOf(typeof(RAWINPUTHEADER))
        );

        IntPtr buffer = Marshal.AllocHGlobal((int)dwSize);
        try
        {
            if (
                GetRawInputData(
                    hRawInput,
                    RID_INPUT,
                    buffer,
                    ref dwSize,
                    (uint)Marshal.SizeOf(typeof(RAWINPUTHEADER))
                ) != dwSize
            )
            {
                Debug.LogError("GetRawInputData does not return the expected size.");
                return;
            }

            RAWINPUT raw = (RAWINPUT)Marshal.PtrToStructure(buffer, typeof(RAWINPUT));

            if (raw.header.dwType == RIM_TYPEKEYBOARD)
            {
                ushort virtualKey = raw.keyboard.VKey;
                uint scanCode = raw.keyboard.MakeCode;
                uint flags = raw.keyboard.Flags;

                Debug.Log(
                    $"Device: {raw.header.hDevice} frame: {Time.frameCount} guiFrameCount: {guiFrameCount} inputString: {inputString}"
                );

                if (virtualKey != 0)
                {
                    string keyValue = GetKeyValue(virtualKey, scanCode, flags);
                    Debug.Log($"Key Pressed: {keyValue}, Device: {raw.header.hDevice}");

                    // Get device name
                    uint pcbSize = 0;
                    GetRawInputDeviceInfo(
                        raw.header.hDevice,
                        RIDI_DEVICENAME,
                        IntPtr.Zero,
                        ref pcbSize
                    );

                    if (pcbSize > 0)
                    {
                        StringBuilder deviceName = new StringBuilder((int)pcbSize);
                        if (
                            GetRawInputDeviceInfo(
                                raw.header.hDevice,
                                RIDI_DEVICENAME,
                                deviceName,
                                ref pcbSize
                            ) > 0
                        )
                        {
                            Debug.Log($"Device Name: {deviceName}");
                        }
                    }
                }
                else
                {
                    Debug.LogError("VirtualKey is 0.");
                }
            }
            else
            {
                Debug.LogError("Raw input type is not keyboard.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error processing raw input: {ex.Message}");
        }
        finally
        {
            Marshal.FreeHGlobal(buffer);
        }
    }

    private string GetKeyValue(uint virtualKey, uint scanCode, uint flags)
    {
        StringBuilder keyName = new StringBuilder(256);
        byte[] keyboardState = new byte[256];
        if (ToUnicode(virtualKey, scanCode, keyboardState, keyName, keyName.Capacity, flags) > 0)
        {
            return keyName.ToString();
        }
        // Fallback to hex representation if ToUnicode fails
        return $"VK_{virtualKey:X2}";
    }
}
