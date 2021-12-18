using System.Threading.Tasks;
using DisposePattern.Database;
using Microsoft.AspNetCore.Mvc;

namespace DisposePattern
{
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        [HttpGet("disposable")]
        public Task<int> DisposableRepositoryTest([FromServices] DisposableRepository disposableRepository)
        {
            return disposableRepository.TestConnection();
        }
    }
}