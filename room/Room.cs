using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using flaui2.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flaui2.room
{
    public class Room
    {
        public static void ClickRoomLeave(Window w)
        {
            var btnLeave = w.FindFirstDescendant(cf => cf.ByAutomationId("BtnLeave")).AsButton();
            btnLeave.Invoke();
            Common.Sleep(500);
        }

        public static void MessageBoxRoomLeave(Application app, string name, UIA3Automation automation)
        {
            Common.MessageBox(app, "방 나가기", "정말로 방 나갈테야??", name, automation);
        }
    }
}
