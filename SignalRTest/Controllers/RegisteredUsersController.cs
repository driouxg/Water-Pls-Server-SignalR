using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SignalRTest.DataAccess;
using SignalRTest.Domain;
using SignalRTest.Domain.Dto;
using SignalRTest.Domain.Entity;
using SignalRTest.Domain.VO;
using SignalRTest.Hubs;
using SignalRTest.Singleton;

namespace SignalRTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RegisteredUsersController : ControllerBase
    {
        private readonly ILogger<RegisteredUsersController> _logger;
        private readonly IHubContext<RequestWaterHub> _requestorsHubContext;
        private readonly ConnectionMap<UsernameVo> _requestorConnections;
        private readonly WaterDbContext _dbContext;

        public RegisteredUsersController(ILogger<RegisteredUsersController> logger, 
            IHubContext<RequestWaterHub> requestorsHubContext, WaterDbContext dbContext)
        {
            _logger = logger;
            _requestorsHubContext = requestorsHubContext;
            _requestorConnections = RequestorConnectionSingleton.Instance;
            _dbContext = dbContext;
        }

        [HttpGet("candidates")]
        public async Task<List<CandidateDto>> GetActiveCandidates()
        {
            List<CandidateDto> candidates = new List<CandidateDto>();

            _logger.LogInformation($"Number of active requestor connections: {_requestorConnections.Count()}");

            foreach (var activeRequestorName in _requestorConnections.Keys())
            {
                GeoCoordinatesEntity entity;
                try
                {
                    entity = _dbContext.GeoCoordinates.Single(x => x.Username == activeRequestorName._value);

                    candidates.Add(new CandidateDto(activeRequestorName._value, new GeoCoordinatesDto()
                    {
                        latitude = entity.latitude,
                        longitude = entity.longitude
                    }));
                }
                catch (System.InvalidOperationException)
                {
                    _logger.LogWarning($"Failed to find requestor '{activeRequestorName._value}' in active connections.");
                }
            }

            _logger.LogInformation($"Sending request back with {candidates.Count} CandidateDto's");
            return candidates;
        }

        [HttpGet("destination")]
        public async Task<GeoCoordinatesDto> GetUsersLocation(UsernameDto usernameDto)
        {
            GeoCoordinatesEntity entity = _dbContext.GeoCoordinates.Single(x => x.Username == usernameDto.Username);

            return new GeoCoordinatesDto()
            {
                latitude = entity.latitude,
                longitude = entity.longitude
            };
        }
    }
}
