using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Veloci.Logic.Services.Tracks.Models;

namespace Veloci.Logic.Services.Tracks;

public class WebTrackFetcher : ITrackFetcher
{
    private readonly string[] _blackListedMaps =
    {
        "Apartment",
        "CombatPractice",
        "MiniWarehouse",
        "NightClub",
        "Sportbar",
        "House",
        "Office",
        "Sports Hall",
        "Basketball Stadium",
        "Library",
        "Office Complex",
        "Slovenia Krvavec",
        "Karting Track",
        "Night Factory",
        "La Mothe", //premium
        "Castle Sneznik", //premium
        "Drift Track", //premium
    };

    public async Task<List<ParsedMapModel>> FetchMapsAsync()
    {
        var web = new HtmlWeb();
        var doc = await web.LoadFromWebAsync("https://www.velocidrone.com/leaderboards");
        var nodes = doc.DocumentNode.SelectNodes("//a[@class='scenery-card']");
        var maps = nodes.Select(ParseMapNode)
            .Where(x => x is not null)
            .ToList();

        var tasks = maps.Select(FetchMapTracksAsync).ToArray();
        await Task.WhenAll(tasks);

        return maps;
    }

    private async Task FetchMapTracksAsync(ParsedMapModel mapModel)
    {
        var web = new HtmlWeb();
        var doc = await web.LoadFromWebAsync(mapModel.Url);
        var nodes = doc.DocumentNode.SelectNodes("//div[@class='track-grid__li']");

        foreach (var node in nodes)
        {
            var track = ParseTrackNode(node);
            if (track == null) continue;
            mapModel.Tracks.Add(track);
            track.Map = mapModel;
        }
    }

    private ParsedMapModel? ParseMapNode(HtmlNode node)
    {
        var nameNode = node.ParentNode
            .Descendants("div")
            .FirstOrDefault(a => a.GetAttributeValue("class", "").Contains("scenery-card__title"));

        var mapName = nameNode.InnerText;

        if (_blackListedMaps.Contains(mapName))
            return null;

        var url = node.GetAttributeValue("href", null);
        var mapId = GetMapId(url);

        return new ParsedMapModel
        {
            Name = mapName,
            Id = mapId,
            Url = url
        };
    }

    private ParsedTrackModel? ParseTrackNode(HtmlNode node)
    {
        var nameNode = node
            .Descendants("a")
            .FirstOrDefault();

        var trackName = nameNode.InnerText;

        var url = nameNode.GetAttributeValue("href", null);
        var trackId = GetTrackId(url);

        return new ParsedTrackModel
        {
            Name = trackName,
            Id = trackId,
            Url = url
        };
    }

    private int GetMapId(string url)
    {
        const string pattern = @"/(\d+)/";
        var match = Regex.Match(url, pattern);

        if (!match.Success)
        {
            throw new Exception("Map id not found");
        }

        var mapIdString = match.Groups[1].Value;

        if (!int.TryParse(mapIdString, out var mapId))
        {
            throw new Exception("Map id parse error");
        }

        return mapId;
    }

    private int GetTrackId(string url)
    {
        const string pattern = @"(\d+)";
        var regex = new Regex(pattern);
        var matches = regex.Matches(url);

        if (matches.Count < 2)
        {
            throw new Exception("Track id not found");
        }

        var mapIdString = matches[1].Groups[1].Value;

        if (!int.TryParse(mapIdString, out var mapId))
        {
            throw new Exception("Track id parse error");
        }

        return mapId;
    }
}
