using DispoDataAssistant.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;

namespace DispoDataAssistant.Services.Implementations
{
    public class UserPromptService : BaseService, IUserPromptService
    {

        public UserPromptService(ILogger logger) : base(logger)
        {
        }

        public string PromptForNewTabName()
        {
            throw new NotImplementedException();
        }
    }
}
