using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PositionTracking.Data;
using Microsoft.EntityFrameworkCore;
using PositionTracking.Engine;

namespace PositionTracking.WebApi.Controllers
{
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
        public ActionResult<dynamic> GetRank(int id)
        {
            const SearchEngineType searchEngine = SearchEngineType.GoogleWeb;


            var keyword = _dbContext.Keywords
                .Where(k => k.KeywordId == id)
                .Include(k => k.Project)
                .First();

            var result = 96;

            //var result = Resolver.GetRank(
            //    keyword.Value,
            //    keyword.Language,
            //    keyword.Location,
            //    keyword.Project.Paths,
            //    searchEngine);

            if (result == 0)
            {
                return NotFound();
            }

            keyword.Ratings = new List<KeywordRating>()
            {
                new KeywordRating(result, searchEngine)
            };

            _dbContext.SaveChanges();

            return new ActionResult<dynamic>(new
            {
                KeywordId = keyword.KeywordId,
                Rating = result
            });

        }

    }

}
