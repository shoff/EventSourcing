namespace EventSourcing.Sample.Web.Controllers
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Domain.Commands;
    using Domain.Queries;
    using Microsoft.AspNetCore.Mvc;
    using Transactions.Contracts.Accounts.Commands;
    using Transactions.Contracts.Accounts.Queries;
    using Transactions.Contracts.Accounts.ValueObjects;

    [Route("api/[controller]")]
    public class AccountsController: Controller
    {
        private readonly ICommandBus commandBus;
        private readonly IQueryBus queryBus;

        public AccountsController(ICommandBus commandBus, IQueryBus queryBus)
        {
            this.commandBus = commandBus;
            this.queryBus = queryBus;
        }

        // GET api/values
        [HttpGet]
        public Task<AccountSummary> Get(Guid accountId)
        {
            return queryBus.Send<GetAccount, AccountSummary>(new GetAccount(accountId));
        }

        // POST api/values
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.InsufficientStorage)]
        public Task Post([FromForm] CreateNewAccount command)
        {
            return commandBus.Send(command);
        }
    }
}
