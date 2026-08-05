using Microsoft.AspNetCore.Mvc;
using Akkhor.Infrastructure.Data;


[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{

    private readonly ApplicationDbContext _context;


    public TestController(ApplicationDbContext context)
    {
        _context = context;
    }



    [HttpGet]
    public IActionResult Get()
    {

        var count = _context.Users.Count();


        return Ok(new
        {
            Message = "Database Connected",
            Users = count
        });

    }
}