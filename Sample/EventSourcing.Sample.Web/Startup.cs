namespace EventSourcing.Sample.Web
{
    using System.Collections.Generic;
    using Clients.Contracts.Clients.Commands;
    using Clients.Contracts.Clients.Events;
    using Clients.Contracts.Clients.Queries;
    using Clients.Contracts.Clients.ValueObjects;
    using Clients.Domain.Clients.Handlers;
    using Clients.Storage;
    using Domain.Commands;
    using Domain.Events;
    using Domain.Queries;
    using Marten;
    using MediatR;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Swashbuckle.AspNetCore.Swagger;
    using Transactions.Contracts.Accounts.Commands;
    using Transactions.Contracts.Accounts.Events;
    using Transactions.Contracts.Accounts.Queries;
    using Transactions.Contracts.Accounts.ValueObjects;
    using Transactions.Contracts.Transactions.Commands;
    using Transactions.Contracts.Transactions.Events;
    using Transactions.Domain.Accounts;
    using Transactions.Domain.Accounts.Handlers;
    using Transactions.Domain.Clients.Handlers;
    using Transactions.Views.Accounts.AccountSummary;
    using Transactions.Views.Accounts.AllAccountsSummary;
    using Transactions.Views.Accounts.Handlers;
    using Transactions.Views.Clients;

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Event Sourcing Example", Version = "v1" });
            });

            services.AddTransient<IAccountNumberGenerator, RandomAccountNumberGenerator>();

            ConfigureMediator(services);

            ConfigureMarten(services);

            ConfigureEf(services);

            ConfigureCqrs(services);
        }

        private static void ConfigureCqrs(IServiceCollection services)
        {
            services.AddScoped<ICommandBus, CommandBus>();
            services.AddScoped<IQueryBus, QueryBus>();
            services.AddScoped<IEventBus, EventBus>();

            services.AddScoped<IRequestHandler<CreateNewAccount, Unit>, AccountCommandHandler>();
            services.AddScoped<IRequestHandler<MakeTransfer, Unit>, AccountCommandHandler>();
            services.AddScoped<IRequestHandler<GetAccounts, IEnumerable<AccountSummary>>, GetAccountsHandler>();
            services.AddScoped<IRequestHandler<GetAccount, AccountSummary>, GetAccountHandler>();

            services.AddScoped<INotificationHandler<ClientCreated>, ClientsEventHandler>();
            services.AddScoped<INotificationHandler<ClientUpdated>, ClientsEventHandler>();
            services.AddScoped<INotificationHandler<ClientDeleted>, ClientsEventHandler>();

            services.AddScoped<IRequestHandler<CreateClient, Unit>, ClientsCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateClient, Unit>, ClientsCommandHandler>();
            services.AddScoped<IRequestHandler<DeleteClient, Unit>, ClientsCommandHandler>();

            services.AddScoped<IRequestHandler<GetClients, List<ClientListItem>>, EventSourcing.Sample.Clients.Views.Clients.ClientsQueryHandler>();
            services.AddScoped<IRequestHandler<GetClient, ClientItem>, EventSourcing.Sample.Clients.Views.Clients.ClientsQueryHandler>();
            services.AddScoped<IRequestHandler<GetClientView, ClientView>, ClientsQueryHandler>();
        }

        private void ConfigureMarten(IServiceCollection services)
        {
            services.AddScoped(sp =>
            {
                var documentStore = DocumentStore.For(options =>
                {
                    var config = Configuration.GetSection("EventStore");
                    var connectionString = config.GetValue<string>("ConnectionString");
                    var schemaName = config.GetValue<string>("Schema");

                    options.Connection(connectionString);
                    options.AutoCreateSchemaObjects = AutoCreate.All;
                    options.Events.DatabaseSchemaName = schemaName;
                    options.DatabaseSchemaName = schemaName;

                    options.Events.InlineProjections.AggregateStreamsWith<Account>();
                    options.Events.InlineProjections.Add(new AllAccountsSummaryViewProjection());
                    options.Events.InlineProjections.Add(new AccountSummaryViewProjection());
                    options.Events.InlineProjections.Add(new ClientsViewProjection());

                    options.Events.AddEventType(typeof(NewAccountCreated));
                    options.Events.AddEventType(typeof(NewInflowRecorded));
                    options.Events.AddEventType(typeof(NewOutflowRecorded));

                    options.Events.AddEventType(typeof(ClientCreated));
                    options.Events.AddEventType(typeof(ClientUpdated));
                    options.Events.AddEventType(typeof(ClientDeleted));
                });

                return documentStore.OpenSession();
            });
        }

        private void ConfigureEf(IServiceCollection services)
        {
            services.AddDbContext<ClientsDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("ClientsDatabase")));
        }

        private static void ConfigureMediator(IServiceCollection services)
        {
            services.AddScoped<IMediator, Mediator>();
            services.AddTransient<ServiceFactory>(sp => t => sp.GetService(t));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Event Sourcing Example V1");
            });

            app.ApplicationServices.GetService<ClientsDbContext>().Database.Migrate();
        }
    }
}
