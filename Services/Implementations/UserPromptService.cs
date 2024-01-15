using DispoDataAssistant.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
