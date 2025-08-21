using System.Threading.Tasks;
using AlexandreHtrb.AvaloniaUITest;
using Avalonia.Controls;
using Avalonia.Automation.Peers;

namespace avalonia_todo_test.Extensions
{
    public static class ButtonExtensions
    {
        /// <summary>
        /// 更可靠地模拟按钮点击，优先触发 Click 事件，然后尝试 Command。
        /// </summary>
        public static async Task ClickOnReliably(this Button button)
        {
            // 方法1: 尝试使用 AutomationPeer 调用 Invoke (最接近真实用户点击)
            var peer = ButtonAutomationPeer.CreatePeerForElement(button);
            if (peer is ButtonAutomationPeer buttonPeer  )
            {
                buttonPeer.Invoke();
                await UITestActions.WaitAfterActionAsync(); // 等待UI更新
                return;
            }

            // 方法2: 如果没有 AutomationPeer，回退到触发 Click 事件 (如果可能)
            // Avalonia 的 Button.Click 事件通常由内部逻辑触发，直接调用比较困难

            // 方法3: 最后回退到执行 Command (库的默认行为)
            button.Command?.Execute(button.CommandParameter);
            await UITestActions.WaitAfterActionAsync();
        }
    }
}