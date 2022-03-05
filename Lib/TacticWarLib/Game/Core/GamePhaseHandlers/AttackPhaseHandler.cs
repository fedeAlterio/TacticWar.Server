using TacticWar.Lib.Game.Configuration.Abstractions;
using TacticWar.Lib.Game.Core.Pipeline.Middlewares.Data;
using TacticWar.Lib.Game.GamePhases.PhaseInfo;
using TacticWar.Lib.Game.Map;
using TacticWar.Lib.Game.Players;
using TacticWar.Lib.Game.Table;
using TacticWar.Lib.Utils;

namespace TacticWar.Lib.Game.Core.GamePhaseHandlers
{
    public class AttackPhaseHandler
    {
        // Events
        public event Action<AttackPhaseInfo>? AttackPhase;
        public event Action<AttackInfo>? TerritoryConquered;
        public event Action? AttackSkipped;
        public event Action<AttackInfo>? PlayerDefeated;



        // Private fields
        private readonly TurnInfo _turnInfo;
        private readonly IGameConfiguration _gameConfiguration;
        private readonly GameTable _gameTable;



        // Initialization
        public AttackPhaseHandler(GameTable gameTable, TurnInfo turnInfo, IGameConfiguration gameConfiguration)
        {
            _turnInfo = turnInfo;
            _gameConfiguration = gameConfiguration;
            _gameTable = gameTable;
        }



        // Properties
        private bool WaitingForDefence => CurrentAttackPhaseInfo?.DefenceDice == null;
        private AttackPhaseInfo? CurrentAttackPhaseInfo { get; set; }



        // Core
        public void StartAttackPhase()
        {
            InvokeAttackPhase(new());
        }


        public void Attack(Territory from, Territory to, int attackDice)
        {
            var dice = _gameTable.Attack(_turnInfo.CurrentActionPlayer!, from, to, attackDice);
            var (pFrom, pTo) = (PlayerTerritoryFromTerritory(from), PlayerTerritoryFromTerritory(to));
            var info = new AttackPhaseInfo
            (
                attackDice: dice,
                attackTerritory: pFrom,
                defenceTerritory: pTo
            );

            _turnInfo.CurrentActionPlayer = PlayerFromTerritory(pTo.Territory);
            InvokeAttackPhase(info);
        }

        public void Defend()
        {
            var attackInfo = _gameTable.Defend();
            var info = AttackPhaseInfo.FromAttackinfo(attackInfo);
            _turnInfo.CurrentActionPlayer = _turnInfo.CurrentTurnPlayer;
            var territoryConquered = info?.DefenceTerritory?.Armies == 0;
            if (territoryConquered)
                OnTerritoryConquered(attackInfo);
            InvokeAttackPhase(info!);
        }

        public void SkipAttack()
        {
            CurrentAttackPhaseInfo = null;
            InvokeAttackSkipped();
        }



        // Events to Task
        public async Task AttackSkippedAsync() => await this.TaskFromEvent(@this => @this.AttackSkipped);



        // Utils
        private void OnTerritoryConquered(AttackInfo attackInfo)
        {
            var defenderDefeated = attackInfo.Defender.Territories.Count == 0;
            if (defenderDefeated)
                InvokePlayerDeafeated(attackInfo);
            InvokeTerritoryConquered(attackInfo);
        }


        private Player PlayerFromTerritory(Territory territory)
        {
            return _gameTable.Players
                .First(x => x.Territories.ContainsKey(territory));
        }

        private PlayerTerritory PlayerTerritoryFromTerritory(Territory territory)
        {
            var player = PlayerFromTerritory(territory);
            return player.Territories[territory];
        }


        private void InvokePlayerDeafeated(AttackInfo attackInfo)
        {
            PlayerDefeated?.Invoke(attackInfo);
        }

        private void InvokeAttackPhase(AttackPhaseInfo info)
        {
            CurrentAttackPhaseInfo = info;
            AttackPhase?.Invoke(info);
        }

        private async void InvokeTerritoryConquered(AttackInfo info)
        {
            _turnInfo.WaitingForArmiesPlacementAfterAttack = true;
            await Task.Delay(_gameConfiguration.DelayAfterTerritoryConqueredMs);
            TerritoryConquered?.Invoke(info);
        }

        private void InvokeAttackSkipped()
        {
            AttackSkipped?.Invoke();
        }
    }
}
