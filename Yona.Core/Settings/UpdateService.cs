using CommunityToolkit.Mvvm.ComponentModel;
using Octokit;
using System.Globalization;
using Yona.Core.App;
using Yona.Core.Common;
using Yona.Core.Utils.Serializers;

namespace Yona.Core.Settings;

public partial class UpdateService : ObservableObject
{
    private readonly GitHubClient client;
    private readonly SavableFile<Update> latestRelease;

    [ObservableProperty]
    private Update? update;

    public UpdateService(AppService app, Version currentVersion)
    {
        this.CurrentVersion = currentVersion;
        this.latestRelease = new(Path.Join(app.AppDataDir, "release.json"), JsonFileSerializer.Instance);
        this.client = new GitHubClient(new ProductHeaderValue("Yona"));
    }

    public Version CurrentVersion { get; }

    public async Task CheckUpdates(bool forceCheck = false)
    {
        if (this.ShouldCheckGithub() || forceCheck)
        {
            var releaseData = await this.client.Repository.Release.GetLatest("MirrorTuneZM", "Yona");
            this.latestRelease.Data = new()
            {
                Version = releaseData.Name,
                Body = releaseData.Body,
                Url = releaseData.HtmlUrl,
                Date = releaseData.CreatedAt.ToLocalTime().ToString(CultureInfo.CurrentCulture),
            };

            this.latestRelease.Save();
        }

        var latestVersion = new Version(this.latestRelease.Data.Version);
        if (latestVersion.CompareTo(this.CurrentVersion) > 0)
        {
            this.Update = this.latestRelease.Data;
        }
    }

    private bool ShouldCheckGithub()
    {
        if (this.latestRelease.Data.Version == Update.NONE)
        {
            return true;
        }

        var cachedFileInfo = new FileInfo(this.latestRelease.FilePath);
        var currentTime = DateTime.Now;
        var diff = currentTime - cachedFileInfo.LastWriteTime;
        if (diff.TotalHours >= 6)
        {
            return true;
        }

        return false;
    }
}

public class Update
{
    public const string NONE = "NONE";

    public Update()
    {
    }

    public Update(string version, string body, string url)
    {
        this.Version = version;
        this.Body = body;
        this.Url = url;
    }

    public string Version { get; set; } = NONE;

    public string Body { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    public string Date { get; set; } = string.Empty;
}