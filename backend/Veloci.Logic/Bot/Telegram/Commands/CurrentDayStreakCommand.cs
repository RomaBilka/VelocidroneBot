using Veloci.Data.Domain;
using Veloci.Data.Repositories;
using Veloci.Logic.Bot.Telegram.Commands.Core;

namespace Veloci.Logic.Bot.Telegram.Commands;

public class CurrentDayStreakCommand : ITelegramCommand
{
    private readonly IRepository<Pilot> _pilots;

    public CurrentDayStreakCommand(IRepository<Pilot> pilots)
    {
        _pilots = pilots;
    }

    public string[] Keywords => ["/current-day-streak", "/cds"];

    public string Description => "`/current-day-streak {pilotName}` або `/cds {pilotName}` - Current day streak";

    public async Task<string> ExecuteAsync(string[]? parameters)
    {
        if (parameters is null || parameters.Length == 0)
            return "все добре, але не вистачає імені пілота";

        if (parameters.Length > 1)
            return "все добре, але забагато параметрів";

        var pilot = await _pilots.FindAsync(parameters[0]);

        return pilot is null
            ? $"Не знаю такого пілота 😕"
            : $"Current day streak: {pilot.DayStreak}";
    }

    public bool RemoveMessageAfterDelay => false;
}
