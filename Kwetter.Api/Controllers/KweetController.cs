using Kwetter.Api.Dtos;

namespace Kwetter.Api.Controllers;

[Route("api/[controller]")]
public class KweetController : Controller
{

    private readonly IEventSourceRepository<UserId, User> _userRepo;

    public KweetController(IEventSourceRepository<UserId, User> userRepo)
    {
        _userRepo = userRepo;
    }

    [HttpGet()]
    [Route("/user", Name = "GetKweetsForUser")]
    public async Task<IActionResult> GetKweetsAsync()
    {
        var aggregateId = new UserId(Guid.NewGuid());

        User user = await _userRepo.GetByIdAsync(aggregateId);

        if (user == null)
        {
            return NotFound();
        }

        var kweetDtos = user.Kweets.Select(k => KweetDto.MapFrom(k));

        return Ok(kweetDtos);
    }
}
