using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flaui2.common
{
    class Common
    {
        private static string appPath = @"D:\src\vs\wpftalk\bin\Debug\net8.0-windows\talk2.exe";

        public static (List<FlaUI.Core.Application>, List<FlaUI.Core.AutomationElements.Window>) appw(List<string> ids, UIA3Automation automation)
        {
            var _apps = apps(ids);
            var ws = windows(_apps, automation);
            return (_apps, ws);
        }
        public static List<FlaUI.Core.Application> apps(List<string> ids)
        {
            return ids.Select(a => FlaUI.Core.Application.Launch(appPath)).ToList();
        }
        public static List<FlaUI.Core.AutomationElements.Window> windows(List<FlaUI.Core.Application> apps, UIA3Automation automation)
        {
            return apps.Select(a => a.GetMainWindow(automation)).ToList();
        }
        public static void Login(List<FlaUI.Core.AutomationElements.Window> windows, List<string> ids)
        {
            for (int i=0; i < windows.Count; i++)
            {
                var loginButton = windows[i].FindFirstDescendant(cf => cf.ByAutomationId("btnLogin"))?.AsButton();
                var idTextBox = windows[i].FindFirstDescendant(cf => cf.ByAutomationId("txtId"))?.AsTextBox();
                idTextBox.Text = ids[i];
                var pwTextBox = windows[i].FindFirstDescendant(cf => cf.ByAutomationId("txtPw"))?.AsTextBox();
                pwTextBox.Text = ids[i];
                loginButton.Invoke();
            }
            Sleep(1000);
        }

        public static void MessageBox(Application app, string title, string text, string name, UIA3Automation automation)
        {
            var messageBox = app.GetMainWindow(automation).ModalWindows.FirstOrDefault(w => w.Title == title);
            if (messageBox != null)
            {
                // 5. 메시지 텍스트 확인
                var textElement = messageBox.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Text));
                Assert.AreEqual(text, textElement?.AsLabel()?.Text);

                var cancleButton = messageBox.FindFirstDescendant(cf => cf.ByName(name))?.AsButton();
                cancleButton.Invoke();
            }
            else
            {
                Assert.Fail();
            }
            Common.Sleep(1000);
        }

        public static void UserMenuClick(FlaUI.Core.AutomationElements.Window mainWindow)
        {
            var menuUser = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("menuUser"));
            menuUser.Click();
            Sleep(500);
        }
        public static void ChatMenuClick(List<FlaUI.Core.AutomationElements.Window> ws)
        {
            foreach (var w in ws)
            {
                w.Focus();
                var menuChat = w.FindFirstDescendant(cf => cf.ByAutomationId("menuChat"));
                menuChat.Click();
            }
            Sleep(1000);
        }
        public static void SettingMenuClick(FlaUI.Core.AutomationElements.Window mainWindow)
        {
            var menuSetting = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("menuSetting"));
            menuSetting.Click();
            Sleep(500);
        }

        public static void Sleep(int t)
        {
            System.Threading.Thread.Sleep(t);
        }

        public static void Close(List<FlaUI.Core.Application> apps)
        {
            apps.ForEach(a => a.Close());
        }
    }
}
