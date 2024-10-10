using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Novena.Kiosk
{
    public class NKioskUtility
    {
        public static void OpenTaskManager()
        {
            System.Diagnostics.Process.Start("taskmgr.exe");
        }
    }
}