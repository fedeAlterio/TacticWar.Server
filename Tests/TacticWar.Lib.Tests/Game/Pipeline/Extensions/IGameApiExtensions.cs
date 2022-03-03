using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game;

namespace TacticWar.Lib.Tests.Game.Pipeline.Extensions
{
    public static class IGameApiExtensions
    {
        public static async Task AssertForEachMethodThat(this IGameApi @this, Action setupBeforeCallingNext, Func<bool> assertion, [CallerArgumentExpression("assertion")] string expectedResultExpression = "")
        {
            async Task AssertForEachOfThese(params Expression<Func<Task>>[] actions)
            {
                foreach (var action in actions)
                {
                    setupBeforeCallingNext.Invoke();
                    await action.Compile().Invoke();
                    assertion.Invoke().Should().BeTrue(because: $"expression is: {expectedResultExpression} on method {action}") ;
                }
            }

            await AssertForEachOfThese
            (
                () => @this.Start(),
                () => @this.PlayTris(default, default),
                () => @this.PlaceArmies(default, default, default),
                () => @this.SkipPlacementPhase(default),
                () => @this.PlaceArmiesAfterAttack(default, default),
                () => @this.Attack(default, default, default, default),
                () => @this.Defend(default),
                () => @this.SkipAttack(default),
                () => @this.Movement(default, default, default, default),
                () => @this.SkipFreeMove(default),
                () => @this.TerminateGame()
            );
        }
    }
}
