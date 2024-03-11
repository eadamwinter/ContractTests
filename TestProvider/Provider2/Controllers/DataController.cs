using Microsoft.AspNetCore.Mvc;
using Provider2.Entities;

namespace Provider2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DataController : ControllerBase
{
    [HttpGet]
    public ActionResult<DataResponse> Get()
    {
        // These are lines which should succeeded or failed all contract tests. Uncomment the chosen one, comment the other and change reponse type to proper one.

        return new DataResponse { Data = new List<string> { "Value1", "Value2" } };
        //return new WrongDataResponse { Data = new List<int> { 1, 2 } };
    }
}
