using Avalonia.Controls.Notifications;
using Serilog.Core;
using Serilog.Events;
using SukiUI.Controls;
using System;

namespace Yona.Desktop.Common;

internal class ToastSink : ILogEventSink
{
    public void Emit(LogEvent logEvent)
    {
        if (logEvent.Level >= LogEventLevel.Error)
        {
            SukiHost.ShowToast(logEvent.RenderMessage(), logEvent.Exception?.Message ?? string.Empty, NotificationType.Error, TimeSpan.FromSeconds(30));
        }
        else if (logEvent.Level == LogEventLevel.Information)
        {
            SukiHost.ShowToast("Info", logEvent.RenderMessage(), NotificationType.Information, TimeSpan.FromSeconds(5));
        }
    }
}
