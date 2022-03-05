using System.Linq.Expressions;

namespace TacticWar.Lib.Utils
{
    public static class EventToTask
    {
        public static Task TaskFromEvent<T>(this T @this, Expression<Func<T, Action?>> actionExpression)
        {
            _ = @this ?? throw new ArgumentNullException(nameof(@this));
            var memberExpression = (MemberExpression)actionExpression.Body;
            var eventInfo = @this.GetType().GetEvent(memberExpression.Member.Name)!;
            var addMethod =  eventInfo.GetAddMethod();
            var removeMethod = eventInfo.GetRemoveMethod();
            var delegateType = addMethod!.GetParameters()[0].ParameterType;
            var tcs = new TaskCompletionSource<object?>();
            Action? handler = null;
            handler = () =>
            {
                tcs.SetResult(null);
                removeMethod!.Invoke(@this, new [] { handler! });
            };
            addMethod.Invoke(@this, new [] { handler });
            return tcs.Task;
        }
    }
}
