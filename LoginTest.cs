using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flaui2
{
    [TestFixture]
    class LoginTest
    {
        private Application app;
        private UIA3Automation automation;

        [SetUp]
        public void Setup()
        {
            var appPath = @"D:\src\vs\chat\talk2\bin\Debug\net8.0-windows\talk2.exe";
            app = Application.Launch(appPath);
            automation = new UIA3Automation();
        }

        [TearDown]
        public void TearDown()
        {
            automation.Dispose();
            app.Close();
        }

        [Test]
        public void TestLoginButtonExists()
        {
            var mainWindow = app.GetMainWindow(automation);
            var loginButton = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("btnLogin"))?.AsButton();

            NUnit.Framework.Assert.IsNotNull(loginButton, "Login 버튼을 찾을 수 없습니다.");

            System.Threading.Thread.Sleep(2000);
        }

        [Test]
        public void TestLoginButtonExists1()
        {
            var mainWindow = app.GetMainWindow(automation);
            var loginButton = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("btnLogin"))?.AsButton();

            NUnit.Framework.Assert.IsNotNull(loginButton, "Login 버튼을 찾을 수 없습니다.");

            loginButton.Invoke();  // 버튼 클릭

            System.Threading.Thread.Sleep(2000);
        }

        [Test]
        public void TestLoginButtonExists2()
        {
            var mainWindow = app.GetMainWindow(automation);
            var loginButton = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("btnLogin"))?.AsButton();

            NUnit.Framework.Assert.IsNotNull(loginButton, "Login 버튼을 찾을 수 없습니다.");

            loginButton.Invoke();  // 버튼 클릭

            System.Threading.Thread.Sleep(4000);
        }
    }
}
