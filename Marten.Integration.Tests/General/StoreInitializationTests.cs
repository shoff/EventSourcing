using System.Linq;
using Npgsql;
using SharpTestsEx;
using Xunit;

namespace Marten.Integration.Tests.General
{
    public class StoreInitializationTests
    {
        [Fact]
        public void GivenWrongConnectionString_WhenDocumentSessionIsInitialized_ThenConnectionIsCreated()
        {
            var ex = Record.Exception(() =>
            {
                var store = DocumentStore.For("WrongConnectionString");

                ConnectionShouldBeEstablished(store);
            });

            ex.Should().Not.Be.Null();
        }

        [Fact]
        public void GivenProperConnectionString_WhenDocumentSessionIsInitialized_ThenConnectionIsCreated()
        {
            var ex = Record.Exception(() =>
            {
                var store = DocumentStore.For(Settings.ConnectionString);

                ConnectionShouldBeEstablished(store);
            });

            ex.Should().Be.Null();
        }

        [Fact]
        public void GivenProperConnectionString_WhenDocumentSessionIsInitializedWithDifferentShema_ThenConnectionIsCreated()
        {
            var ex = Record.Exception(() =>
            {
                var store = DocumentStore.For(options =>
                {
                    options.Connection(Settings.ConnectionString);
                    options.AutoCreateSchemaObjects = AutoCreate.All;
                    options.Events.DatabaseSchemaName = Settings.SchemaName;
                });

                ConnectionShouldBeEstablished(store);
            });

            ex.Should().Be.Null();
        }

        [Fact]
        public void GivenSettingWithNpgsqlConnection_WhenDocumentSessionIsInitializedWithDifferentShema_ThenConnectionIsCreated()
        {
            var ex = Record.Exception(() =>
            {
                var store = DocumentStore.For(options =>
                {
                    options.Connection(() => new NpgsqlConnection(Settings.ConnectionString));
                    options.AutoCreateSchemaObjects = AutoCreate.All;
                    options.Events.DatabaseSchemaName = Settings.SchemaName;
                });

                ConnectionShouldBeEstablished(store);
            });

            ex.Should().Be.Null();
        }

        [Fact]
        public void GivenProperConnectionString_WhenDocumentSessionIsCreatedAndTransactionIsCreated_ThenConnectionIsCreatedAndItsPossibleToMakeRollback()
        {
            var ex = Record.Exception(() =>
            {
                var store = DocumentStore.For(options =>
                {
                    options.Connection(Settings.ConnectionString);
                    options.AutoCreateSchemaObjects = AutoCreate.All;
                    options.Events.DatabaseSchemaName = Settings.SchemaName;
                });

                using (var session = store.OpenSession())
                {
                    using (var transaction = session.Connection.BeginTransaction())
                    {
                        ConnectionShouldBeEstablished(store);

                        transaction.Rollback();
                    }
                }
            });

            ex.Should().Be.Null();
        }

        private static void ConnectionShouldBeEstablished(IDocumentStore store)
        {
            using (var session = store.OpenSession())
            {
                var events = session.Events.QueryAllRawEvents().ToList();

                events.Should().Not.Be.Null();
            }
        }
    }
}
