using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TacticWar.Lib.Game.Abstractions;
using TacticWar.Lib.Game.Core.Abstractions;
using TacticWar.Lib.Game.Core.Pipeline.Abstractions;
using TacticWar.Lib.Game.Deck;
using TacticWar.Lib.Game.Map;
using TacticWar.Lib.Game.Players;
using TacticWar.Lib.Game.Table.Abstractions;

namespace TacticWar.Lib.Game.Core.Pipeline.Middlewares;
public partial class GameLogging : IGamePipelineMiddleware
{
    readonly INewTurnManager _turnManager;
    readonly IGameTable _gameTable;
    readonly GameStartupInformation _gameStartupInformation;
    readonly ILogger<GameLogging> _logger;

    public GameLogging(INewTurnManager turnManager, IGameTable gameTable, GameStartupInformation gameStartupInformation, ILogger<GameLogging> logger)
    {
        _turnManager = turnManager;
        _gameTable = gameTable;
        _gameStartupInformation = gameStartupInformation;
        _logger = logger;
    }

    string? PlayerName => _turnManager.TurnInfo.CurrentTurnPlayer?.Name;
    int RoomId => _gameStartupInformation.RoomId;

    public Task Start()
    {
        LogStart(_logger, RoomId, PlayerName);
        return Next!();
    }

    public Task PlaceArmies(PlayerColor playerColor, int armies, Territory territory)
    {
        LogPlaceArmies(_logger, RoomId, PlayerName, playerColor, armies, territory.Name);
        return Next!();
    }

    public Task PlayTris(PlayerColor playerColor, IEnumerable<TerritoryCard> cards)
    {
        var tris = string.Join(", ", cards.Select(static x => x.ArmyType));
        LogPlayTris(_logger, RoomId, PlayerName, playerColor, tris);
        return Next!();
    }

    public Task SkipPlacementPhase(PlayerColor playerColor)
    {
        LogSkipPlacement(_logger, RoomId, PlayerName, playerColor);
        return Next!();
    }

    public Task PlaceArmiesAfterAttack(PlayerColor playerColor, int armies)
    {
        LogPlaceArmiesAfterAttack(_logger, RoomId, PlayerName, armies);
        return Next!();
    }

    public Task Attack(PlayerColor color, Territory from, Territory to, int attackDice)
    {
        LogAttack(_logger, RoomId, PlayerName, color, from.Name, to.Name, attackDice);
        return Next!();
    }

    public Task Defend(PlayerColor playerColor)
    {
        LogDefence(_logger, RoomId, PlayerName, playerColor);
        return Next!();
    }

    public Task SkipAttack(PlayerColor playerColor)
    {
        LogSkipAttack(_logger, RoomId, PlayerName, playerColor);
        return Next!();
    }

    public Task Movement(PlayerColor playerColor, Territory from, Territory to, int armies)
    {
        LogMovement(_logger, RoomId, PlayerName, playerColor, from.Name, to.Name);
        return Next!();
    }

    public Task SkipFreeMove(PlayerColor playerColor)
    {
        LogSkipFreeMove(_logger, RoomId, PlayerName, playerColor);
        return Next!();
    }

    public Task TerminateGame()
    {
        LogTerminateGame(_logger, RoomId, PlayerName);
        return Next!(); 
    }

    [LoggerMessage(
        EventId = 0,
        Level = LogLevel.Information,
        Message = "Game started. RoomId: Room: {roomId}, Player name: {playerName}")]
    static partial void LogStart(ILogger logger, int roomId, string? playerName);


    [LoggerMessage(
        EventId = 0,
        Level = LogLevel.Information,
        Message = "Placing Armies. RoomId: Room: {roomId}, Player name: {playerName}, Player color {playerColor}, Armies {armies}, Territory {territoryName}")]
    static partial void LogPlaceArmies(ILogger logger, int roomId, string? playerName, PlayerColor playerColor, int armies, string territoryName);


    [LoggerMessage(
        EventId = 0,
        Level = LogLevel.Information,
        Message = "Playing trist. RoomId: Room: {roomId}, Player name: {playerName}, Player color {playerColor}, Tris: {tris}")]
    static partial void LogPlayTris(ILogger logger, int roomId, string? playerName, PlayerColor playerColor, string tris);


    [LoggerMessage(
        EventId = 0,
        Level = LogLevel.Information,
        Message = "Skipping placement. RoomId: Room: {roomId}, Player name: {playerName}, Player color {playerColor}")]
    static partial void LogSkipPlacement(ILogger logger, int roomId, string? playerName, PlayerColor playerColor);


    [LoggerMessage(
        EventId = 0,
        Level = LogLevel.Information,
        Message = "Placing armies after attack. RoomId: Room: {roomId}, Player name: {playerName}, armies {armies}")]
    static partial void LogPlaceArmiesAfterAttack(ILogger logger, int roomId, string? playerName, int armies);


    [LoggerMessage(
        EventId = 0,
        Level = LogLevel.Information,
        Message = "Attacking a territory. RoomId: Room: {roomId}, Player name: {playerName}, PlayerColor {playerColor}, From {territoryNameFrom}, To {territoryNameTo}, Attack dice {attackDice}")]
    static partial void LogAttack(ILogger logger, int roomId, string? playerName, PlayerColor playerColor, string territoryNameFrom, string territoryNameTo, int attackDice);


    [LoggerMessage(
        EventId = 0,
        Level = LogLevel.Information,
        Message = "Defending a territory. RoomId: Room: {roomId}, Player name: {playerName}, PlayerColor {playerColor}")]
    static partial void LogDefence(ILogger logger, int roomId, string? playerName, PlayerColor playerColor);

    
    [LoggerMessage(
        EventId = 0,
        Level = LogLevel.Information,
        Message = "Skipping attack. RoomId: Room: {roomId}, Player name: {playerName}, PlayerColor {playerColor}")]
    static partial void LogSkipAttack(ILogger logger, int roomId, string? playerName, PlayerColor playerColor);


    [LoggerMessage(
        EventId = 0,
        Level = LogLevel.Information,
        Message = "Moving armies. RoomId: Room: {roomId}, Player name: {playerName}, PlayerColor {playerColor}, From {territoryNameFrom}, To {territoryNameTo}")]
    static partial void LogMovement(ILogger logger, int roomId, string? playerName, PlayerColor playerColor, string territoryNameFrom, string territoryNameTo);


    [LoggerMessage(
        EventId = 0,
        Level = LogLevel.Information,
        Message = "Skipping free move. RoomId: Room: {roomId}, Player name: {playerName}, PlayerColor {playerColor}")]
    static partial void LogSkipFreeMove(ILogger logger, int roomId, string? playerName, PlayerColor playerColor);


    [LoggerMessage(
        EventId = 0,
        Level = LogLevel.Information,
        Message = "Terminating game. RoomId: Room: {roomId}, Player name: {playerName}")]
    static partial void LogTerminateGame(ILogger logger, int roomId, string? playerName);

    public NextPipelineStepDelegate? Next { get; set; }
    public void Initialize()
    {
    }
}
