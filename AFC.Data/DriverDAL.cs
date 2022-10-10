using System;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Cosmos;
using global::AFC.Models;

namespace AFC.Data
{
    public class DriverDAL
    {
        // The Azure Cosmos DB endpoint for running this sample.
        private readonly string EndpointUri = "https://localhost:8081";

        // The primary key for the Azure Cosmos account.
        private readonly string PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

        // The Cosmos client instance
        private CosmosClient cosmosClient;

        // The database we will create
        private Database database;

        // The container we will create.
        private Container container;

        // The name of the database and container we will create
        private string databaseId = "db";
        private string containerId = "items";

        // <Main>
        public async Task Initialize()
        {
            try
            {
                await GetStartedDemoAsync();
            }
            catch (CosmosException de)
            {
                Exception baseException = de.GetBaseException();
            }
            catch (Exception e)
            {
                throw;
            }
        }
        // </Main>

        // <GetStartedDemoAsync>
        public async Task GetStartedDemoAsync()
        {
            // Create a new instance of the Cosmos Client
            this.cosmosClient = new CosmosClient(EndpointUri, PrimaryKey, new CosmosClientOptions() { ApplicationName = "CosmosDBDotnetQuickstart" });
            await this.CreateDatabaseAsync();
            await this.CreateContainerAsync();
            await this.ScaleContainerAsync();
            await this.AddItemsToContainerAsync();
        }
        // </GetStartedDemoAsync>

        // <CreateDatabaseAsync>
        /// <summary>
        /// Create the database if it does not exist
        /// </summary>
        private async Task CreateDatabaseAsync()
        {
            // Create a new database
            this.database = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
        }
        // </CreateDatabaseAsync>

        // <CreateContainerAsync>
        private async Task CreateContainerAsync()
        {
            // Create a new container
            this.container = await this.database.CreateContainerIfNotExistsAsync(containerId, "/location", 400);
        }
        // </CreateContainerAsync>

        // <ScaleContainerAsync>
        private async Task ScaleContainerAsync()
        {
            // Read the current throughput
            int? throughput = await this.container.ReadThroughputAsync();
            if (throughput.HasValue)
            {
                int newThroughput = throughput.Value + 100;
                // Update throughput
                await this.container.ReplaceThroughputAsync(newThroughput);
            }

        }
        // </ScaleContainerAsync>

        // <AddItemsToContainerAsync>
        private async Task AddItemsToContainerAsync()
        {
            for (int i = 1; i <= 100; i++)
            {
                Driver driver = new Driver
                {
                    Id = "Driver" + i,
                    FirstName = "First " + i,
                    LastName = "Last " + i,
                    Location = "Berlin"
                };
                try
                {
                    // Read the item to see if it exists.  
                    ItemResponse<Driver> driverResponse = await this.container.ReadItemAsync<Driver>(driver.Id, new PartitionKey(driver.Location));
                }
                catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    try
                    {
                        // Create an item in the container representing the new driver."
                        ItemResponse<Driver> driverResponse = await this.container.CreateItemAsync<Driver>(driver, new PartitionKey(driver.Location));
                    }
                    catch(Exception ex1)
                    {

                    }
                    
                }
            }
        }
        // </AddItemsToContainerAsync>

        // <QueryItemsAsync>
        /// <summary>
        /// Run a query (using Azure Cosmos DB SQL syntax) against the container
        /// </summary>
        public async Task<List<Driver>> QueryItemsAsync(string city)
        {
            await Initialize(); //initialize the Cosmos DB
            var sqlQueryText = "SELECT * FROM c where c.location='" + city + "'";

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<Driver> queryResultSetIterator = this.container.GetItemQueryIterator<Driver>(queryDefinition);

            List<Driver> drivers = new List<Driver>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Driver> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (Driver driver in currentResultSet)
                {
                    drivers.Add(driver);
                }
            }
            await DeleteDatabaseAndCleanupAsync();
            return drivers;
        }
        // </QueryItemsAsync>

        // <DeleteDatabaseAndCleanupAsync>
        private async Task DeleteDatabaseAndCleanupAsync()
        {
            DatabaseResponse databaseResourceResponse = await this.database.DeleteAsync();
            //Dispose of CosmosClient
            this.cosmosClient.Dispose();
        }
        // </DeleteDatabaseAndCleanupAsync>
    }
}