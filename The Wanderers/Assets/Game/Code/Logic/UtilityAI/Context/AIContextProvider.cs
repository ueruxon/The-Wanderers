namespace Game.Code.Logic.UtilityAI.Context
{
    public class AIContextProvider<TContext> : IContextProvider where TContext : AIContext
    {
        private readonly TContext _aiContext;
        
        public AIContextProvider(TContext context) => 
            _aiContext = context;
        
        public T GetContext<T>() where T : AIContext => 
            _aiContext as T;
    }
    
    public interface IContextProvider
    {
        public T GetContext<T>() where T : AIContext;
    }
}