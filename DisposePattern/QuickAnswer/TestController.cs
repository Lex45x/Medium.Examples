using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuickAnswer.Database;

namespace QuickAnswer
{
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        [HttpGet("simple")]
        public Task<int> SimpleRepositoryTest([FromServices] Repository repository)
        {
            return repository.TestConnection();
        }
    }
}