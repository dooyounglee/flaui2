using FlaUI.Core.AutomationElements;
using flaui2.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flaui2.chat
{
    public class Chat
    {
        public static void OpenFirstRoom(List<Window> wl)
        {
            OpenRoom(wl);
        }

        public static void OpenRoomByRoomNo(List<Window> wl, string roomNo)
        {
            OpenRoom(wl, roomNo);
        }

        public static void OpenRoom(List<Window> wl)
        {
            wl.ForEach(w =>
            {
                w.Focus();
                var chats = w.FindFirstDescendant(cf => cf.ByAutomationId("chats"));
                var chat = chats.FindAllChildren().FirstOrDefault();
                chat.DoubleClick();
            });
        }

        public static void OpenRoom(List<Window> wl, string roomNo)
        {
            wl.ForEach(w =>
            {
                w.Focus();
                var chats = w.FindFirstDescendant(cf => cf.ByAutomationId("chats"));
                var chat = chats.FindFirstDescendant(cf => cf.ByAutomationId(roomNo));
                chat.DoubleClick();
            });
        }

        public static void ClickCreateRoom(List<Window> wl)
        {
            wl.ForEach(w =>
            {
                Common.ClickBtn(w, "BtnCreate");
            });
        }
    }
}
