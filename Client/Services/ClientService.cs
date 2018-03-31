using System.Threading.Tasks;

namespace IgiCore.Client.Services
{
    public abstract class ClientService : IClientService
    {
        private bool Busy { get; set; }

        public async Task Tick(Client client)
        {
            if (this.Busy) return;

            this.Busy = true;
            await OnTick(client);
            this.Busy = false;
        }

        public abstract Task OnTick(Client client);
    }
}
