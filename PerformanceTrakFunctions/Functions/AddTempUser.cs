using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PerformanceTrakFunctions.Security;
using PerformanceTrakFunctions.Repository;
using PerformanceTrakFunctions.Util;

namespace PerformanceTrakFunctions.Functions
{
    public class AddTempUser
    {
        private readonly IAccessTokenProvider _tokenProvider;

        private readonly IUserRepository _userRepository;

        public AddTempUser(IAccessTokenProvider tokenProvider, IUserRepository userRepository)
        {
            _tokenProvider = tokenProvider;
            _userRepository = userRepository;
        }

        [FunctionName("AddTempUser")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "AddTempUser")] HttpRequest req, ILogger log) => NewUser.Add(_tokenProvider, _userRepository, req, log, "TempUsers");
    }
}
