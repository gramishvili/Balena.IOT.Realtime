using System;
using System.Threading.Tasks;
using Balena.IOT.Entity.Entities;

namespace Balena.IOT.MessageQueue
{
    public interface IMessageBroker
    {
        Task BroadcastAsync(IEntity message);
        Task SubscribeAsync<T>(Action<IEntity> action) where T : IEntity;
    }
}