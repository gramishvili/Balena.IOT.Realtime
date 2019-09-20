using System;
using System.Threading.Tasks;
using Balena.IOT.Entity.Entities;

namespace Balena.IOT.MessageQueue
{
    public interface IMessageBroker
    {
        Task BroadcastAsync(IEntity message);
        Task SubscribeAsync<T>(Action<T> action) where T : IEntity;
    }
}