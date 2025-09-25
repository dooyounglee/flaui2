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
        public void IntoRoom()
        {
            var ids = new List<string> { "1", "2", "3" };
            var (apps, wl) = Common.appw(ids, automation);

            Common.Login(wl, ids);
            Common.ChatMenuClick(wl);

            string roomNo = "15";
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
    }
}
