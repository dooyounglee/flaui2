using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flaui2
{
    class Program
    {
        static void Main(string[] args)
        {
            // 자동화 대상 앱 경로
            var appPath = @"D:\src\vs\chat\talk2\bin\Debug\net8.0-windows\talk2.exe";

            // 앱 실행
            using (var app = Application.Launch(appPath))
            {
                // UI Automation 객체 생성 (UIA3 사용)
                using (var automation = new UIA3Automation())
                {
                    // 메인 윈도우 가져오기
                    var mainWindow = app.GetMainWindow(automation);

                    Console.WriteLine($"윈도우 타이틀: {mainWindow.Title}");

                    // 예: "로그인" 버튼 찾기 (AutomationId나 Name으로)
                    var loginButton = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("btnLogin"))?.AsButton();

                    if (loginButton != null)
                    {
                        Console.WriteLine("로그인 버튼 찾음! 클릭합니다.");
                        loginButton.Invoke();  // 버튼 클릭
                    }
                    else
                    {
                        Console.WriteLine("로그인 버튼을 찾지 못했습니다.");
                    }

                    // 잠시 대기 (결과 확인용)
                    System.Threading.Thread.Sleep(2000);
                }

                // 앱 종료
                app.Close();
            }
        }
    }
}
