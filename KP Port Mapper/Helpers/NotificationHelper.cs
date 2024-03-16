using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KP_Port_Mapper.Helpers;
internal class NotificationHelper(Form form)
{
    private readonly Form Form = form;
    private bool _isNotificationShown;
    internal void ShowNotification(string text, int duration, bool isError = false)
    {
        if (_isNotificationShown)
        {
            Form.Controls[0].Text = text;
            return;
        }

        CreateAndShowNewNotification(text, duration, isError);
    }
    private void CreateAndShowNewNotification(string text, int duration, bool isError)
    {
        int width = (int)(Form.Width * 0.75);
        Label notification = new()
        {
            Width = width,
            Location = new Point((Form.Width - width) / 2, 15),
            BackColor = isError ? Color.Red : Color.LimeGreen,
            Text = text
        };

        Form.Controls.Add(notification);
        Form.Controls.SetChildIndex(notification, 0);
        _isNotificationShown = true;

        Task.Run(() =>
        {
            AdjustNotificationHeight(notification, 24);
            Thread.Sleep(duration);
            AdjustNotificationHeight(notification, 0);

            Form.Invoke(new MethodInvoker(() =>
            {
                _isNotificationShown = false;
                Form.Controls.Remove(notification);
                notification.Dispose();
            }));
        });
    }
    private void AdjustNotificationHeight(Label notification, int targetHeight)
    {
        bool increase = targetHeight-notification.Height  > 0;
        int step = increase ? 1 : -1;
        int start = increase ? 0 : notification.Height;

        for (int i = start; increase ? i <= targetHeight : i >= targetHeight; i += step)
        {
            Form.Invoke(new MethodInvoker(() => notification.Height = i));
            Thread.Sleep(5);
        }
    }
}
