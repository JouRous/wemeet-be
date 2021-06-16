using API.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class BaseApiController : ControllerBase
  {
    protected AppDbContext DbContext => (AppDbContext)HttpContext.RequestServices.GetService(typeof(AppDbContext));
    
  }
}
