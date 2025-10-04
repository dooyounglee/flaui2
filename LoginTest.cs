using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using flaui2.common;
using System.Windows.Controls;
using FlaUI.Core.Definitions;
using FlaUI.Core.WindowsAPI;
using FlaUI.Core.Input;
using FlaUI.Core.Tools;
using System.Windows.Interop;
using flaui2.chat;
using flaui2.room;

namespace flaui2
{
    [TestFixture]
    class LoginTest
    {
        private List<Application> apps = new List<Application>();
        private UIA3Automation automation;
        // private List<string> ids = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9" };

        [SetUp]
        public void Setup()
        {
            automation = new UIA3Automation();
        }

        // [TearDown]
        // public void TearDown()
        // {
        //     automation.Dispose();
        //     apps.ForEach(a => a.Close());
        // }

        [Test]
        public void Login()
        {
            var ids = new List<string> { "1", "2", "3" };
            var (apps, wl) = Common.appw(ids, automation);
            Common.Login(wl, ids);

            Common.Close(apps);
        }

        [Test]
        public void GotoChat()
        {
            var ids = new List<string> { "1", "2" };
            var (apps, wl) = Common.appw(ids, automation);
            Common.Login(wl, ids);
            Common.Sleep(2000);

            Common.ChatMenuClick(wl);
            Common.Sleep(5000);

            Common.Close(apps);
        }

        [Test]
        public void IntoRoom([Values("15")] string roomNo)
        {
            var ids = new List<string> { "1", "2", "3" };
            var (apps, wl) = Common.appw(ids, automation);

            Common.Login(wl, ids);
            Common.ChatMenuClick(wl);

            wl.ForEach(w =>
            {
                w.Focus();
                var chats = w.FindFirstDescendant(cf => cf.ByAutomationId("chats"));
                chats.FindAllDescendants().ToList().ForEach(x => System.Diagnostics.Debug.WriteLine(x.ClassName + "," + x.AutomationId));
                var item15 = chats.FindFirstDescendant(cf => cf.ByAutomationId(roomNo));
                item15.DoubleClick();
            });

            apps.ForEach(a => a.GetAllTopLevelWindows(automation)
                               .FirstOrDefault(w => w.Properties.AutomationId.ValueOrDefault == $"RoomNo_{roomNo}")
                               .Close());

            Common.Close(apps);
        }

        [Test]
        public void 채팅치면_다른방에도_전파([Values("15")] string roomNo)
        {
            var ids = new List<string> { "1", "2", "3" };
            var (apps, wl) = Common.appw(ids, automation);

            Common.Login(wl, ids);
            Common.ChatMenuClick(wl);

            // 채팅방 켜기
            wl.ForEach(w =>
            {
                w.Focus();
                var chats = w.FindFirstDescendant(cf => cf.ByAutomationId("chats"));
                chats.FindAllDescendants().ToList().ForEach(x => System.Diagnostics.Debug.WriteLine(x.ClassName + "," + x.AutomationId));
                var item15 = chats.FindFirstDescendant(cf => cf.ByAutomationId(roomNo));
                item15.DoubleClick();
            });

            List<Window> rooms = new List<Window>();
            apps.ForEach(a => rooms.Add(a.GetAllTopLevelWindows(automation)
                               .FirstOrDefault(w => w.Properties.AutomationId.ValueOrDefault == $"RoomNo_{roomNo}")));

            // 채팅입력
            string msg = "이두영";
            var txt = rooms[0].FindFirstDescendant(cf => cf.ByAutomationId("txt")).AsTextBox();
            txt.Enter(msg);
            // Keyboard.Press(VirtualKeyShort.RETURN);   // 누르기 (누르고 있음)
            // Keyboard.Release(VirtualKeyShort.RETURN); // 떼기
            Keyboard.Type(VirtualKeyShort.RETURN); // 한 번 누르기
            Common.Sleep(1000);

            // 각 채팅방 마지막 채팅 확인
            rooms.ForEach(w =>
            {
                var chats = w.FindFirstDescendant(cf => cf.ByAutomationId("chats"));
                var chatItems = chats?.FindAllChildren();
                var lastChat = chatItems?.LastOrDefault();
                var chat = lastChat.FindFirstDescendant(cf => cf.ByAutomationId("chat")).AsTextBox();
                Assert.AreEqual(msg, chat.Text);
            });

            // Common.Close(apps);
        }

        [Test]
        public void 채팅창_첫번째방_나가기_취소()
        {
            var ids = new List<string> { "1" };
            var (apps, wl) = Common.appw(ids, automation);

            Common.Login(wl, ids);
            Common.ChatMenuClick(wl);

            // 채팅방 첫번째방 열기
            Chat.OpenFirstRoom(wl);

            Window room = apps[0].GetAllTopLevelWindows(automation).FirstOrDefault();
            string roomAutomationId = room.AutomationId;

            Room.ClickRoomLeave(room);
            Room.MessageBoxRoomLeave(apps[0], "취소", automation);

            Assert.IsNotNull(apps[0].GetAllTopLevelWindows(automation).FirstOrDefault(cf => cf.AutomationId == roomAutomationId));
        }

        [Test]
        public void 채팅창_나가기_취소([Values("9")] string roomNo)
        {
            var ids = new List<string> { "1" };
            var (apps, wl) = Common.appw(ids, automation);

            Common.Login(wl, ids);
            Common.ChatMenuClick(wl);

            // 채팅방 켜기
            Chat.OpenRoomByRoomNo(wl, roomNo);

            List<Window> rooms = new List<Window>();
            apps.ForEach(a => rooms.Add(a.GetAllTopLevelWindows(automation)
                               .FirstOrDefault(w => w.Properties.AutomationId.ValueOrDefault == $"RoomNo_{roomNo}")));

            rooms.ForEach(r =>
            {
                Room.ClickRoomLeave(r);
                Room.MessageBoxRoomLeave(apps[0], "취소", automation);

                Assert.NotNull(apps[0].GetAllTopLevelWindows(automation).FirstOrDefault(cf => cf.AutomationId == $"RoomNo_{roomNo}"));
            });
        }

        [Test]
        public void 채팅창_첫번째방_나가기_확인()
        {
            var ids = new List<string> { "1" };
            var (apps, wl) = Common.appw(ids, automation);

            Common.Login(wl, ids);
            Common.ChatMenuClick(wl);

            // 채팅방 첫번째방 열기
            Chat.OpenFirstRoom(wl);

            Window room = apps[0].GetAllTopLevelWindows(automation).FirstOrDefault();
            string roomAutomationId = room.AutomationId;

            Room.ClickRoomLeave(room);
            Room.MessageBoxRoomLeave(apps[0], "확인", automation);

            Assert.IsNull(apps[0].GetAllTopLevelWindows(automation).FirstOrDefault(cf => cf.AutomationId == roomAutomationId));
        }

        [Test]
        public void 채팅창_나가기_확인([Values("9")] string roomNo)
        {
            var ids = new List<string> { "1" };
            var (apps, wl) = Common.appw(ids, automation);

            Common.Login(wl, ids);
            Common.ChatMenuClick(wl);

            // 채팅방 켜기
            Chat.OpenRoomByRoomNo(wl, roomNo);

            Window room = apps[0].GetAllTopLevelWindows(automation)
                               .FirstOrDefault(w => w.Properties.AutomationId.ValueOrDefault == $"RoomNo_{roomNo}");

            Room.ClickRoomLeave(room);
            Room.MessageBoxRoomLeave(apps[0], "확인", automation);

            Assert.IsNull(apps[0].GetAllTopLevelWindows(automation).FirstOrDefault(cf => cf.AutomationId == $"RoomNo_{roomNo}"));
        }

        [Test]
        public void 채팅창_2명방_만들기()
        {
            var ids = new List<string> { "1" };
            var (apps, wl) = Common.appw(ids, automation);

            Common.Login(wl, ids);
            Common.ChatMenuClick(wl);

            Chat.ClickCreateRoom(wl);

            apps.ForEach(a =>
            {

            });
        }

        [Test]
        public void 채팅창_3명방_만들기()
        {
            var ids = new List<string> { "1" };
            var (apps, wl) = Common.appw(ids, automation);

            Common.Login(wl, ids);
            Common.ChatMenuClick(wl);

            Chat.ClickCreateRoom(wl);

            apps.ForEach(a =>
            {
                var w_UserPopup = a.GetAllTopLevelWindows(automation).FirstOrDefault(w => w.Properties.AutomationId == "UserPopupView");
                var users = w_UserPopup.FindFirstDescendant(cf => cf.ByAutomationId("users"));
                users.FindFirstDescendant(cf => cf.ByAutomationId("2")).Click();
                Keyboard.Press(VirtualKeyShort.CONTROL);
                users.FindFirstDescendant(cf => cf.ByAutomationId("3")).Click();
                users.FindFirstDescendant(cf => cf.ByAutomationId("4")).Click();

                var btnSelect = w_UserPopup.FindFirstDescendant(cf => cf.ByAutomationId("BtnSelect")).AsButton();
                btnSelect.Invoke();
            });
        }

        // [Test]
        public void 채팅창_켜진채로_메인창끄면_알림문자()
        {
            var ids = new List<string> { "1" };
            var (apps, wl) = Common.appw(ids, automation);

            Common.Login(wl, ids);
            Common.ChatMenuClick(wl);

            // 채팅방 켜기
            string roomNo = "15";
            wl.ForEach(w =>
            {
                w.Focus();
                var chats = w.FindFirstDescendant(cf => cf.ByAutomationId("chats"));
                // chats.FindAllDescendants().ToList().ForEach(x => System.Diagnostics.Debug.WriteLine(x.ClassName + "," + x.AutomationId));
                var item15 = chats.FindFirstDescendant(cf => cf.ByAutomationId(roomNo));
                item15.DoubleClick();
            });

            List<Window> rooms = new List<Window>();
            apps.ForEach(a => rooms.Add(a.GetAllTopLevelWindows(automation)
                               .FirstOrDefault(w => w.Properties.AutomationId.ValueOrDefault == $"RoomNo_{roomNo}")));

            // 메인창 끄기
            System.Diagnostics.Debug.WriteLine(1);
            Common.Close(apps); System.Diagnostics.Debug.WriteLine(2);
            Common.Sleep(1000); System.Diagnostics.Debug.WriteLine(3);

            wl[0].FindAllDescendants().ToList().ForEach(x => System.Diagnostics.Debug.WriteLine(x.ClassName + "," + x.AutomationId));
            // 
            // var allWindows = apps[0].GetAllTopLevelWindows(automation);
            // System.Diagnostics.Debug.WriteLine(allWindows.ToList().Count);
            // foreach (var win in allWindows)
            // {
            //     System.Diagnostics.Debug.WriteLine($"Title: {win.Title}, ClassName: {win.ClassName}, AutomationId: {win.AutomationId}");
            // }

            System.Diagnostics.Debug.WriteLine(4);
            // 메시지 박스 뜰 때까지 기다림
            var messageBox = Retry.WhileNull(() =>
                automation.GetDesktop().FindFirstDescendant(cf =>
                    cf.ByControlType(ControlType.Window)
                      .And(cf.ByName("열린 채팅창이 있어요"))),
                timeout: TimeSpan.FromSeconds(5)).Result;
            System.Diagnostics.Debug.WriteLine(5);
            Assert.IsNotNull(messageBox, "메시지박스를 찾지 못했습니다.");
            System.Diagnostics.Debug.WriteLine(6);
            Assert.IsNotNull(messageBox, "메시지 박스가 떠야 합니다.");

            // var okButton = messageBox.FindFirstDescendant(cf => cf.ByControlType(ControlType.Button));
            // 
            // Assert.IsNotNull(okButton, "확인 버튼을 찾을 수 없습니다.");
            // 
            // okButton.AsButton().Invoke();
            // 
            // // 5. 메시지 텍스트 확인
            // var textElement = messageBox.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Text));
            // Assert.IsTrue(textElement?.Name.Contains("열린 채팅창이 있어요") ?? false, "올바른 메시지가 표시되어야 합니다.");
            // 
            // // focus확인
            // var targetWindow = apps[0].GetMainWindow(automation);
            // 
            // // ✅ nullable 대응
            // AutomationElement focusedElement = automation.FocusedElement();
            // bool isFocused = false;
            // if (focusedElement != null)
            // {
            //     var focusedHandle = focusedElement.Properties.NativeWindowHandle.ValueOrDefault;
            //     var targetHandle = targetWindow.Properties.NativeWindowHandle.ValueOrDefault;
            //     isFocused = focusedHandle == targetHandle;
            // }
            // Assert.IsTrue(isFocused, "해당 창이 포커스를 가져야 합니다.");
        }
    }
}
