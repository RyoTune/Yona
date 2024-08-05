using System;
using System.IO;
using Yona.Library.ViewModels.Dashboard.Home;

namespace Yona.Desktop.DesignData;

public class DesignHomeViewModel : HomeViewModel
{
    public DesignHomeViewModel()
    {
        this.Templates.Add(new() { Name = "Blank Project" });
        this.Templates.Add(new() { Name = "Persona 3 Reload BGM (PC)" });
        this.Templates.Add(new() { Name = "Persona 5 Royal BGM (PC)" });
        this.Templates.Add(new() { Name = "Persona 4 Golden BGM (PC)" });
        this.Templates.Add(new() { Name = "Persona 3 Portable BGM (PC)" });

        for (int i = 0; i < 20; i++)
        {
            this.Projects.Add(new() { Name = $"Project {i + 1}", FilePath = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "file.txt") });
            this.Templates.Add(new() { Name = $"Template {i + 1} but like with a REALLY long name", Author = "Author Name" });
        }
    }
}
