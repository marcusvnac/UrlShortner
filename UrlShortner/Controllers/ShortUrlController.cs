using Microsoft.AspNetCore.Mvc;
using ShortherUrlCore.Business;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShortnerUrl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShortUrlController : ControllerBase
    {
        private readonly IShortnerUrlBS shortnerUrlBS;

        public ShortUrlController(IShortnerUrlBS shortnerUrlBS)
        {
            this.shortnerUrlBS = shortnerUrlBS;
        }

        // GET api/<ShortUrlController>/5
        [HttpGet("{originalUrl}")]
        public async Task<string> Get(string originalUrl)
        {
            var shortnedUrl = await this.shortnerUrlBS.Process(originalUrl);
            return $"www.shorturl.com/{shortnedUrl}";
        }
    }
}
