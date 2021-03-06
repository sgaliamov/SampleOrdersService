﻿using System.Threading.Tasks;
using OrdersService.BusinessLogic.Contracts.Commands;

namespace OrdersService.BusinessLogic
{
    public sealed class CommandDispatcher : ICommandDispatcher
    {
        private readonly ICommandHandlerFactory _factory;

        public CommandDispatcher(ICommandHandlerFactory factory)
        {
            _factory = factory;
        }

        public async Task<string> ExecuteAsync<T>(T command) where T : ICommand
        {
            var handler = _factory.Resolve<T>();

            return await handler.ExecuteAsync(command).ConfigureAwait(false);
        }
    }
}
