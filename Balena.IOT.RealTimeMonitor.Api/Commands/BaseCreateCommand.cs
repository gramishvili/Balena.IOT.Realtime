using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Balena.IOT.RealTimeMonitor.Api.Commands
{
    public abstract class BaseCreateCommand
    {
        public string CommandId { get; set; }

        public async virtual Task<ValidationResult> ValidateAsync()
        {
            return null;
        }
    }
}