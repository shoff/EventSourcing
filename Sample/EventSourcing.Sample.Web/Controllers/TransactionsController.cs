using System.Threading.Tasks;
using Domain.Commands;
using Domain.Queries;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EventSourcing.Sample.Web.Controllers
{
    using Transactions.Contracts.Transactions.Commands;

    [Route("api/[controller]")]
    public class TransactionsController: Controller
    {
        private readonly ICommandBus _commandBus;
        private readonly IQueryBus _queryBus;

        public TransactionsController(ICommandBus commandBus, IQueryBus queryBus)
        {
            _commandBus = commandBus;
            _queryBus = queryBus;
        }

        // POST api/values
        [HttpPost]
        public Task Post([FromBody]MakeTransfer command)
        {
            return _commandBus.Send(command);
        }
    }
}
