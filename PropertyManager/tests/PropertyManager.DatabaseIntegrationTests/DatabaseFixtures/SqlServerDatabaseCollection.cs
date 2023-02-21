using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyManager.Shared.DatabaseIntegrationTests.DatabaseFixtures
{
    [CollectionDefinition("DatabaseCollection")]
    public class DatabaseCollection : ICollectionFixture<SqlServerDatabaseFixture>
    {
    }
}
