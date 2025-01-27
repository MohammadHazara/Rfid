using Microsoft.Azure.Cosmos;
using Rfid.Core.Interfaces.Repositories;
using Rfid.Core.Models;
using Rfid.Data.Infrastructure;

namespace Rfid.Data.Repositories
{
    public class RfidRepository : IRfidRepository
    {
        private readonly CosmosClient _cosmosClient;
        private readonly Container _container;

        public RfidRepository(CosmosClient cosmosClient, CosmosContainerConfig config)
        {
            _cosmosClient = cosmosClient;
            _container = _cosmosClient.GetContainer(config.DatabaseName, config.ContainerName);
        }

        public async Task<Guid> AddAsync(RfidTokenDTO token)
        {
            var result = await _container.CreateItemAsync(token, new PartitionKey());
            return result.Resource.Id;
        }

        public async Task<RfidTokenDTO> GetByIdAsync(Guid id)
        {
            try
            {
                ItemResponse<RfidTokenDTO> response = await _container.ReadItemAsync<RfidTokenDTO>(id.ToString(), new PartitionKey(id.ToString()));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }
    }
}
