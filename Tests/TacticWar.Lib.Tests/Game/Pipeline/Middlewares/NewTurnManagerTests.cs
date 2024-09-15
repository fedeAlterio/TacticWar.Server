using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Abstractions;
using TacticWar.Lib.Game.Core.Pipeline.Middlewares;
using TacticWar.Lib.Game.Deck;
using TacticWar.Lib.Game.Deck.Abstractions;
using TacticWar.Lib.Game.Deck.Objectives.Abstractions;
using TacticWar.Lib.Game.GamePhases;
using TacticWar.Lib.Game.GamePhases.PhaseInfo;
using TacticWar.Lib.Game.Players;
using TacticWar.Lib.Game.Table;
using TacticWar.Lib.Tests.Attributes;
using TacticWar.Lib.Tests.Game.Pipeline.TestsUtils;
using TacticWar.Test.TacticWar.Lib.Tests.Utils;
using Xunit.Abstractions;

namespace TacticWar.Test.TacticWar.Lib.Tests.Game.Pipeline.Middlewares
{
    public class NewTurnManagerTests
    {
        readonly ITestOutputHelper _output;

        public NewTurnManagerTests(ITestOutputHelper output)
        {
            _output = output;
        }


        [FactFor(nameof(TurnManager))]
        public void Should_StartWithPlacementPhase()
        {
            var turnManager = BuildTurnManager();
            var turnInfo = turnManager.TurnInfo;
            turnInfo.CurrentTurnPlayer.Should().Be(turnInfo.CurrentActionPlayer);
            turnInfo.CurrentPhase.Should().Be(GamePhase.ArmiesPlacement);
        }


        [FactFor(nameof(TurnManager.Start))]
        public async Task Should_CyliclyNotifyPlacementPhaseAtStart()
        {
            // Setup
            await using var exceptionCatcher = new ExceptionCatcher();

            var startPhaseEndedEventTriggered = false;
            var turnManager = BuildTurnManager(out var gameTable);
            var players = gameTable.Players.ToList();
            var turnInfo = turnManager.TurnInfo;
            turnManager.StartPhaseEnded += () => startPhaseEndedEventTriggered = true;
            turnManager.ArmiesPlacementPhase += exceptionCatcher.Action<ArmiesPlacementInfo>(async placementInfo =>
            {
                if (turnManager.IsStartPhaseEnded)
                    return;

                turnInfo.CurrentTurnPlayer.Should().Be(turnInfo.CurrentActionPlayer);
                var player = players.FirstOrDefault(x => x.Color == turnInfo.CurrentTurnPlayer!.Color);
                if (player != null)
                    players.Remove(player);
                var territory = turnInfo.CurrentActionPlayer!.Territories.First().Territory;
                await turnManager.PlaceArmies(turnInfo.CurrentActionPlayer.Color, 1, territory);
            });
            var startPhaseCompleted = turnManager.StartPhaseEndedAsync();



            // Assertions
            turnManager.IsGameStarted.Should().BeFalse();
            turnManager.IsStartPhaseEnded.Should().BeFalse();
            await turnManager.Start();
            await startPhaseCompleted;


            startPhaseEndedEventTriggered.Should().BeTrue();
            turnManager.IsGameStarted.Should().BeTrue();
            players.Should().BeEmpty();


            var minArmies = gameTable.Players.Min(x => x.ArmiesCount);
            var maxArmies = gameTable.Players.Max(x => x.ArmiesCount);
            Math.Abs(minArmies - maxArmies).Should().BeLessThanOrEqualTo(1);
            var allPlayersShouldHaveAtLeastOneArmy = gameTable.Players.All(x => x.ArmiesCount >= 1);
            turnManager.IsStartPhaseEnded.Should().BeTrue();
        }


        [FactFor(nameof(TurnManager))]
        public async Task Should_CorrectlyOrderTurnPhases()
        {
            // Setup
            await using var exceptionCatcher = new ExceptionCatcher();
            var turnManager = BuildTurnManager();
            var turnInfo = turnManager.TurnInfo;

            bool armiesPlacementPhaseInvoked = false, attackPhaseInvoked = false, freeMovementPhaseInvoked = false;


            // Armies Placement Phase Handler
            var armiesPlacementHandler = exceptionCatcher.Action<ArmiesPlacementInfo>(async placementInfo =>
            {
                if (turnManager.TurnNumber >= 1)
                    return;

                if (turnManager.IsStartPhaseEnded)
                {
                    turnInfo.CurrentPhase.Should().Be(GamePhase.ArmiesPlacement);
                    armiesPlacementPhaseInvoked.Should().BeFalse();
                    attackPhaseInvoked.Should().BeFalse();
                    freeMovementPhaseInvoked.Should().BeFalse();
                    armiesPlacementPhaseInvoked = true;
                }
                await turnManager.PlaceArmies(turnInfo.CurrentActionPlayer!.Color, placementInfo.ArmiesToPlace, turnInfo.CurrentActionPlayer.Territories.First().Territory);
            });
            turnManager.ArmiesPlacementPhase += armiesPlacementHandler;


            // Attack Phase Handler
            var attackPhaseHandler = exceptionCatcher.Action<AttackPhaseInfo>(async attackInfo =>
            {
                if (turnManager.TurnNumber >= 1)
                    return;

                turnInfo.CurrentPhase.Should().Be(GamePhase.Attack);
                armiesPlacementPhaseInvoked.Should().BeTrue();
                attackPhaseInvoked.Should().BeFalse();
                freeMovementPhaseInvoked.Should().BeFalse();
                attackPhaseInvoked = true;
                await turnManager.SkipAttack(turnInfo.CurrentActionPlayer!.Color);
            });
            turnManager.AttackPhase += attackPhaseHandler;


            // Free Move Phase Handler
            var freeMovePhaseHandler = exceptionCatcher.Action<FreeMovePhaseInfo>(async freeMoveInfo =>
            {
                if (turnManager.TurnNumber >= 1)
                    return;

                turnInfo.CurrentPhase.Should().Be(GamePhase.FreeMove);
                armiesPlacementPhaseInvoked.Should().BeTrue();
                attackPhaseInvoked.Should().BeTrue();
                freeMovementPhaseInvoked.Should().BeFalse();
                freeMovementPhaseInvoked = true;
                await turnManager.SkipFreeMove(turnInfo.CurrentActionPlayer!.Color);
            });
            turnManager.FreeMovePhase += freeMovePhaseHandler;


            // Starting game
            var startPhaseTask = turnManager.StartPhaseEndedAsync();
            var turnEnded = turnManager.TurnEndedAsync();
            await turnManager.Start();
            await startPhaseTask;


            // Assertions       
            await turnEnded;
            armiesPlacementPhaseInvoked.Should().BeTrue();
            attackPhaseInvoked.Should().BeTrue();
            freeMovementPhaseInvoked.Should().BeTrue();
        }


        [FactFor(nameof(TurnManager.Attack))]
        public async Task ShouldCorrectlyAttackAndDefendWhenDefenceWins()
        {
            await using var exceptionCatcher = new ExceptionCatcher();
            var diceRollerMock = new Mock<IDiceRoller>();
            diceRollerMock.SetupSequence(x => x.RollDice()).Returns(1).Returns(6);
            var turnManager = BuildTurnManager(out var gameTable, diceRollerMock.Object);
            var turnInfo = turnManager.TurnInfo;

            Player? attacker = null;
            Player? defender = null;
            PlayerTerritory? attackFromPlayerTerritory = null;
            PlayerTerritory? attackToPlayerTerritory = null;

            int oldAttackerArmies = 0;
            int oldDefenerArmies = 0;

            var defenceCalled = false;
            var tcs = new TaskCompletionSource<object?>();

            // Event Handlers

            // Armies Placement Phase Handler
            var armiesPlacementHandler = exceptionCatcher.Action<ArmiesPlacementInfo>(async placementInfo =>
            {
                await turnManager.PlaceArmies(turnInfo.CurrentActionPlayer!.Color, placementInfo.ArmiesToPlace, turnInfo.CurrentActionPlayer.Territories.First().Territory);
            });
            turnManager.ArmiesPlacementPhase += armiesPlacementHandler;


            // Attack Phase Handler            
            var attackPhaseHandler = exceptionCatcher.Action<AttackPhaseInfo>(async attackInfo =>
            {
                AssertAttackPhaseInfoIsCorrect(attackInfo);
                if (defenceCalled)
                {
                    turnInfo.CurrentTurnPlayer!.Color.Should().Be(attacker!.Color);
                    turnInfo.CurrentActionPlayer!.Color.Should().Be(attacker!.Color);
                    attackFromPlayerTerritory!.Armies.Should().Be(oldAttackerArmies - 1);
                    attackToPlayerTerritory!.Armies.Should().Be(oldDefenerArmies);
                    await turnManager.SkipAttack(turnInfo.CurrentTurnPlayer!.Color);
                    tcs.SetResult(null);
                }
                else if (gameTable.WaitingForDefence)
                {
                    turnInfo.CurrentTurnPlayer!.Color.Should().Be(attacker!.Color);
                    turnInfo.CurrentActionPlayer!.Color.Should().Be(defender!.Color);
                    defenceCalled = true;
                    attackToPlayerTerritory.Should().NotBeNull();
                    attacker.Should().NotBeNull();
                    defender.Should().NotBeNull();
                    attackFromPlayerTerritory.Should().NotBeNull();
                    await turnManager.Defend(defender!.Color);
                }
                else
                {
                    attacker = gameTable.Players.First(p => p.Color == turnInfo.CurrentActionPlayer!.Color);
                    attackFromPlayerTerritory = attacker.Territories.Values.First(t => t.Armies > 1);
                    var attackToTerritory = attackFromPlayerTerritory.Territory.Neighbors.Where(t => !attacker.Territories.ContainsKey(t)).First();
                    attackToPlayerTerritory = gameTable.Players.First(p => p.Territories.ContainsKey(attackToTerritory)).Territories[attackToTerritory];
                    defender = gameTable.Players.First(p => p.Territories.ContainsKey(attackToPlayerTerritory!.Territory));
                    (oldAttackerArmies, oldDefenerArmies) = (attackFromPlayerTerritory.Armies, attackToPlayerTerritory.Armies);

                    await turnManager.Attack(turnInfo.CurrentActionPlayer!.Color, attackFromPlayerTerritory.Territory, attackToPlayerTerritory.Territory, 1);
                }
            });

            turnManager.AttackPhase += attackPhaseHandler;

            // Utils
            void AssertAttackPhaseInfoIsCorrect(AttackPhaseInfo attackPhaseInfo)
            {
                attackPhaseInfo.AttackTerritory.Should().Be(attackFromPlayerTerritory);
                attackPhaseInfo.DefenceTerritory.Should().Be(attackToPlayerTerritory);
            }

            await turnManager.Start();
            await tcs.Task;
        }


        [FactFor(nameof(TurnManager.Attack))]
        public async Task ShouldCorrectlyAttackAndDefendWhenAttackWins()
        {
            await using var exceptionCatcher = new ExceptionCatcher();
            var diceRollerMock = new Mock<IDiceRoller>();
            var shouldDiceRollSix = true;
            diceRollerMock.Setup(x => x.RollDice()).Returns(() =>
            {
                var ret = shouldDiceRollSix ? 6 : 1;
                shouldDiceRollSix = !shouldDiceRollSix;
                return ret;
            });
            var turnManager = BuildTurnManager(out var gameTable, diceRollerMock.Object);
            var turnInfo = turnManager.TurnInfo;

            Player? attacker = null;
            Player? defender = null;
            PlayerTerritory? attackFromPlayerTerritory = null;
            PlayerTerritory? attackToPlayerTerritory = null;

            var defenceCalled = false;
            var tcs = new TaskCompletionSource<object?>();

            // Event Handlers

            // Armies Placement Phase Handler
            var armiesPlacementHandler = exceptionCatcher.Action<ArmiesPlacementInfo>(async placementInfo =>
            {
                await turnManager.PlaceArmies(turnInfo.CurrentActionPlayer!.Color, placementInfo.ArmiesToPlace, turnInfo.CurrentActionPlayer.Territories.First().Territory);
            });
            turnManager.ArmiesPlacementPhase += armiesPlacementHandler;


            // Attack Phase Handler
            var attackPhaseHandler = exceptionCatcher.Action<AttackPhaseInfo>(async attackInfo =>
            {
                AssertAttackPhaseInfoIsCorrect(attackInfo);
                if (gameTable.WaitingForDefence)
                {
                    defenceCalled = true;
                    await turnManager.Defend(defender!.Color);
                }
                else
                {
                    if (defenceCalled)
                    {
                        defenceCalled = false;
                        if (tcs.Task.IsCompleted)
                            return;

                        if (attackToPlayerTerritory!.Armies == 0)
                            return;
                    }

                    attacker = gameTable.Players.First(p => p.Color == turnInfo.CurrentActionPlayer!.Color);
                    attackFromPlayerTerritory = attacker.Territories.Values.First(t => t.Armies > 1);
                    var attackToTerritory = attackFromPlayerTerritory.Territory.Neighbors.Where(t => !attacker.Territories.ContainsKey(t)).First();
                    attackToPlayerTerritory = gameTable.Players.First(p => p.Territories.ContainsKey(attackToTerritory)).Territories[attackToTerritory];
                    defender = gameTable.Players.First(p => p.Territories.ContainsKey(attackToPlayerTerritory!.Territory));

                    await turnManager.Attack(turnInfo.CurrentActionPlayer!.Color, attackFromPlayerTerritory.Territory, attackToPlayerTerritory.Territory, 1);
                }
            });

            // Utils
            void AssertAttackPhaseInfoIsCorrect(AttackPhaseInfo attackPhaseInfo)
            {
                attackPhaseInfo.AttackTerritory.Should().Be(attackFromPlayerTerritory);
                attackPhaseInfo.DefenceTerritory.Should().Be(attackToPlayerTerritory);
            }

            turnManager.AttackPhase += attackPhaseHandler;


            // Placement After Attack Phase
            var placementAfterAttackHandler = exceptionCatcher.Action<AttackInfo>(async attackInfo =>
            {
                turnManager.AttackPhase -= attackPhaseHandler;
                turnInfo.CurrentPhase.Should().Be(GamePhase.PlacementAfterAttack);
                turnInfo.CurrentActionPlayer.Should().Be(attackInfo.Attacker);
                turnInfo.CurrentTurnPlayer.Should().Be(attackInfo.Attacker);
                attackFromPlayerTerritory = null;
                attackToPlayerTerritory = null;
                defenceCalled = false;
                await turnManager.PlaceArmiesAfterAttack(turnInfo.CurrentTurnPlayer!.Color, 1);
                //turnInfo.CurrentActionPlayer.Should().Be(attackInfo.Attacker);
                //turnInfo.CurrentTurnPlayer.Should().Be(attackInfo.Attacker);
                turnInfo.CurrentPhase.Should().Be(GamePhase.Attack);
                tcs.SetResult(null);

            });

            turnManager.PlacementAfterAttackPhase += placementAfterAttackHandler;


            await turnManager.Start();
            await tcs.Task;

        }


        [FactFor(nameof(TurnManager))]
        public async Task Sould_CorrectlyDropATris()
        {
            await using var exceptionCatcher = new ExceptionCatcher();
            var turnManager = BuildTurnManager(out var gameTable);
            var turnInfo = turnManager.TurnInfo;
            var player = gameTable.Players.First(p => turnInfo.CurrentActionPlayer!.Color == p.Color);
            bool trisDropped = false;
            int oldArmiesCount = 0;
            List<TerritoryCard>? tris = null;
            var tcs = new TaskCompletionSource<object?>();
            var startPhase = turnManager.StartPhaseEndedAsync();
            var trisArmies = 0;

            // Forcing a tris on player deck

            bool HasTris(Player player)
            {
                var potentialTris = player.Cards.Where(c => c.ArmyType == ArmyType.Artillery).ToList();
                var hasTris = potentialTris.Count >= 3;
                tris = potentialTris.Take(3).ToList();
                trisArmies = 4 + 2 * tris.Count(c => player.Territories.ContainsKey(c.Territory));
                return hasTris;
            }

            while (!HasTris(player))
                player.DrawCardFrom(gameTable.Deck);


            // Setup Event Handlers
            var armiesPlacementHandler = exceptionCatcher.Action<ArmiesPlacementInfo>(async placementInfo =>
            {
                if (!startPhase.IsCompleted)
                {
                    await turnManager.PlaceArmies(turnInfo.CurrentActionPlayer!.Color, placementInfo.ArmiesToPlace, turnInfo.CurrentActionPlayer.Territories.First().Territory);
                }
                else if (trisDropped)
                {
                    turnInfo.CurrentPhase.Should().Be(GamePhase.ArmiesPlacement);
                    placementInfo.ArmiesToPlace.Should().Be(oldArmiesCount + trisArmies);
                    tcs.SetResult(null);
                }
                else
                {
                    oldArmiesCount = placementInfo.ArmiesToPlace;
                    trisDropped = true;
                    await turnManager.PlayTris(turnInfo.CurrentActionPlayer!.Color, tris!);
                }
            });

            turnManager.ArmiesPlacementPhase += armiesPlacementHandler;


            // Start
            await turnManager.Start();
            await tcs.Task;
        }


        [FactFor(nameof(TurnManager))]
        public async Task Should_TransferAllCardsOnPlayerDefeated()
        {
            await using var exceptionCatcher = new ExceptionCatcher();
            var diceRollerMock = new Mock<IDiceRoller>();
            var forceSix = true;
            diceRollerMock.Setup(x => x.RollDice()).Returns(() =>
            {
                if (forceSix)
                {
                    forceSix = false;
                    return 6;
                }
                return 1;
            });
            var turnManager = BuildTurnManager(out var gameTable, diceRollerMock.Object);            
            var turnInfo = turnManager.TurnInfo;
            var tcs = new TaskCompletionSource<object?>();
            var startPhase = turnManager.StartPhaseEndedAsync();
            var playerThatShouldLose = gameTable.Players.First();
            var playerDefeated = false;
            Player? attacker = null;
            TerritoryCard? defenderCard = null;

            foreach (var player in gameTable.Players)
                player.DrawCardFrom(gameTable.Deck);


            // Setup Event Handlers
            var armiesPlacementHandler = exceptionCatcher.Action<ArmiesPlacementInfo>(async placementInfo =>
            {
                if (playerDefeated)
                    return;

                var player = gameTable.Players.First(p => p.Color == turnInfo.CurrentActionPlayer!.Color);
                var playerTerritory = player.Territories.Values.OrderBy(t => t.Armies).FirstOrDefault();
                if(playerTerritory != null)
                    await turnManager.PlaceArmies(player.Color, 1, playerTerritory.Territory);
            });

            turnManager.ArmiesPlacementPhase += armiesPlacementHandler;


            var attackPhaseHandler = exceptionCatcher.Action<AttackPhaseInfo>(async attackInfo =>
            {
                if (playerDefeated)
                    return;
                var isPlayerThatShouldLose = turnInfo.CurrentActionPlayer!.Color == playerThatShouldLose.Color;
                if (isPlayerThatShouldLose)
                    if (gameTable.WaitingForDefence)
                        await turnManager.Defend(playerThatShouldLose.Color);
                    else
                        await turnManager.SkipAttack(playerThatShouldLose.Color);
                else
                {
                    attacker = gameTable.Players.First(p => p.Color == turnInfo.CurrentActionPlayer!.Color);
                    var query = from territory in attacker.Territories
                                where territory.Value.Armies > 1
                                from neighbor in territory.Key.Neighbors
                                where playerThatShouldLose.Territories.ContainsKey(neighbor)
                                select new { AttackFrom = territory.Key, AttackTo = neighbor };
                    var attackFromTo = query.FirstOrDefault();
                    if (attackFromTo is null)
                        await turnManager.SkipAttack(playerThatShouldLose.Color);
                    else
                    {
                        forceSix = true;
                        defenderCard = playerThatShouldLose.Cards.First();
                        await turnManager.Attack(attacker.Color, attackFromTo.AttackFrom, attackFromTo.AttackTo, 1);
                    }
                }
            });

            turnManager.AttackPhase += attackPhaseHandler;


            var freeMovePhaseHandler = exceptionCatcher.Action<FreeMovePhaseInfo>(async freeMoveInfo =>
            {
                if (playerDefeated)
                    return;
                await turnManager.SkipFreeMove(turnInfo.CurrentActionPlayer!.Color);
            });

            turnManager.FreeMovePhase += freeMovePhaseHandler;


            var placementAfterAttackHandler = exceptionCatcher.Action<AttackInfo>(async attackInfo =>
            {
                if (playerDefeated)
                    return;
                await turnManager.PlaceArmiesAfterAttack(turnInfo.CurrentActionPlayer!.Color, 1);
                turnInfo.CurrentPhase.Should().Be(GamePhase.Attack);
            });

            turnManager.PlacementAfterAttackPhase += placementAfterAttackHandler;


            var playerDefatedHandler = exceptionCatcher.Action<AttackInfo>(async attackInfo =>
            {
                playerDefeated = true;
                var defender = attackInfo.Defender;
                defender.Territories.Count.Should().Be(0);
                defender.Cards.Should().BeEmpty();
                attacker!.Cards.Should().Contain(defenderCard!);
                tcs.SetResult(null);
            });

            turnManager.PlayerDefeated += playerDefatedHandler;

            // Start
            await turnManager.Start();
            await tcs.Task;
        }



        // Utils

        TurnManager BuildTurnManager(out GameTable gameTable, IDiceRoller? diceRoller = null, IDeck<IObjective>? objectivesDeck = null)
        {
            gameTable = ObjectsBuilder.NewGameTable(diceRoller, objectivesDeck);
            var trisManager = ObjectsBuilder.NewTrisManager(gameTable);
            var gameConfiguration = ObjectsBuilder.NewGameConfiguration();
            var turnManager = new TurnManager(gameTable, trisManager, gameConfiguration, new())
            {
                Next = () => Task.CompletedTask
            };
            return turnManager;
        }

        TurnManager BuildTurnManager(IDiceRoller? diceRoller = null) => BuildTurnManager(out _, diceRoller);
    }
}
