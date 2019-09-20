using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Balena.IOT.Entity.Entities;

namespace Balena.IOT.MessageQueue
{
    public class InternalMessageBroker : IMessageBroker
    {
        private readonly ConcurrentDictionary<Type, ConcurrentBag<Action<IEntity>>> RegisteredHandlers =
            new ConcurrentDictionary<Type, ConcurrentBag<Action<IEntity>>>();

        public async Task BroadcastAsync(IEntity message)
        {
            if (message == null)
                return;

            if (RegisteredHandlers.Count == 0)
                return;
            
            var messageType = message.GetType();

            if (!RegisteredHandlers.TryGetValue(messageType, out var handlers) || handlers.Count == 0)
                return;

            foreach (var handler in handlers)
            {
                try
                {
                    handler.Invoke(message);
                }
                catch (TargetInvocationException ex)
                {
                    if (ex.InnerException != null)
                        throw ex.InnerException;

                    throw;
                }
            }

            await Task.CompletedTask;
        }

        public async Task SubscribeAsync<T>(Action<IEntity> action) where T : IEntity
        {
            var type = typeof(T);
            
            if (RegisteredHandlers.ContainsKey(type))
            {
                RegisteredHandlers[type].Add(action);
            }
            else
            {
                var bag = new ConcurrentBag<Action<IEntity>>();
                bag.Add(action);
                RegisteredHandlers.TryAdd(type, bag);
            }
        }
    }
}