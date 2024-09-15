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
        readonly TurnInfo _turnInfo;
        readonly IGameConfiguration _gameConfiguration;
        readonly GameTable _gameTable;



        // Initialization
        public AttackPhaseHandler(GameTable gameTable, TurnInfo turnInfo, IGameConfiguration gameConfiguration)
        {
            _turnInfo = turnInfo;
            _gameConfiguration = gameConfiguration;
            _gameTable = gameTable;
        }



        // Properties
        bool WaitingForDefence => CurrentAttackPhaseInfo?.DefenceDice == null;
        AttackPhaseInfo? CurrentAttackPhaseInfo { get; set; }



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
        void OnTerritoryConquered(AttackInfo attackInfo)
        {
            var defenderDefeated = attackInfo.Defender.Territories.Count == 0;
            if (defenderDefeated)
                InvokePlayerDeafeated(attackInfo);
            InvokeTerritoryConquered(attackInfo);
        }

        Player PlayerFromTerritory(Territory territory)
        {
            return _gameTable.Players
                .First(x => x.Territories.ContainsKey(territory));
        }

        PlayerTerritory PlayerTerritoryFromTerritory(Territory territory)
        {
            var player = PlayerFromTerritory(territory);
            return player.Territories[territory];
        }

        void InvokePlayerDeafeated(AttackInfo attackInfo)
        {
            PlayerDefeated?.Invoke(attackInfo);
        }

        void InvokeAttackPhase(AttackPhaseInfo info)
        {
            CurrentAttackPhaseInfo = info;
            AttackPhase?.Invoke(info);
        }

        async void InvokeTerritoryConquered(AttackInfo info)
        {
            _turnInfo.WaitingForArmiesPlacementAfterAttack = true;
            await Task.Delay(_gameConfiguration.DelayAfterTerritoryConqueredMs);
            TerritoryConquered?.Invoke(info);
        }

        void InvokeAttackSkipped()
        {
            AttackSkipped?.Invoke();
        }
    }
}
