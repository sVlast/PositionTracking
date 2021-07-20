using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PositionTracking.Data;
using PositionTracking.Engine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;





namespace PositionTracking.WebApi.Controllers
{
    [AllowCrossSite]

    [ApiController]
    [Route("[controller]")]
    public class EngineController : ControllerBase
    {

    private readonly ILogger<EngineController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public EngineController(ILogger<EngineController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _dbContext = context;
        }

        [HttpGet("getinfo")]
        public ActionResult GetInfo()
        {
            return new OkResult();
        }

        [HttpGet("getrank/{id}")]
        public async Task<ActionResult<dynamic>> GetRank(Guid id)
        {
            const SearchEngineType searchEngine = SearchEngineType.GoogleWeb;


            var keyword = await _dbContext.Keywords
                .Include(k => k.Project)
                .FirstAsync(k => k.KeywordId == id);
                  

            //var result = 96;

            var result = await Resolver.GetRankAsync(
                keyword.Value,
                keyword.Language,
                keyword.Location,
                keyword.Project.Paths,
                searchEngine,
                _logger);


            keyword.Ratings = new List<KeywordRating>()
            {
                new KeywordRating(result, searchEngine)
            };

            await _dbContext.SaveChangesAsync();

            return new ActionResult<dynamic>(new
            {
                KeywordId = keyword.KeywordId,
                Rating = result
            });

        }

    }

}
