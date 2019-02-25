using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SignalRTest.Domain;
using SignalRTest.Domain.Dto;
using SignalRTest.Domain.VO;
using SignalRTest.Hubs;
using SignalRTest.Singleton;

namespace SignalRTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisteredUsersController : ControllerBase
    {
        private readonly ILogger<RegisteredUsersController> _logger;
        private readonly IHubContext<RequestWaterHub> _requestorsHubContext;
        private readonly ConnectionMap<UsernameVo> _requestorConnections;

        public RegisteredUsersController(ILogger<RegisteredUsersController> logger, IHubContext<RequestWaterHub> requestorsHubContext)
        {
            _logger = logger;
            _requestorsHubContext = requestorsHubContext;
            _requestorConnections = RequestorConnectionSingleton.Instance;
        }

        [HttpGet("candidates")]
        public async Task<List<CandidateDto>> GetActiveCandidates()
        {
            //List<CandidateVo> candidates = new List<CandidateVo>();
            List<CandidateDto> candidates = new List<CandidateDto>();

            _logger.LogInformation($"Number of active requestor connections: {_requestorConnections.Count()}");

            foreach (var activeRequestorName in _requestorConnections.Keys())
            {
                //candidates.Add(new CandidateVo(activeRequestorName, new GeoCoordinatesVo(100.0123123f, 32.123123f)));
                candidates.Add(new CandidateDto(activeRequestorName._value, new GeoCoordinatesDto(100.0123123f, 32.123123f)));
            }
            _logger.LogInformation($"Sending request back with {candidates.Count} CandidateDto's");
            return candidates;
        }
    }
}
