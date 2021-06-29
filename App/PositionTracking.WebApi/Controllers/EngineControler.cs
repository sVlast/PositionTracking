using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PositionTracking.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EngineControler : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<EngineControler> _logger;

        public EngineControler(ILogger<EngineControler> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        
        
    }
}
