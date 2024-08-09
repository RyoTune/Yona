using System;
using System.IO;
using Yona.Core.Projects.Models;

namespace Yona.Desktop.DesignData;

internal class DesignProject : ProjectBundle
{
    public DesignProject()
        : base(Path.Join(AppDomain.CurrentDomain.BaseDirectory, "design-project.yaml"))
    {
        this.Data.Name = "Test Project";
    }
}
