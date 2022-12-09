using Kwetter.Domain.Commands;

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
    [Route("user", Name = "GetKweetsForUser")]
    public async Task<IActionResult> GetKweetsAsync()
    {
        var aggregateId = new UserId(Guid.NewGuid());

        User user = await _userRepo.GetByIdAsync(aggregateId);

        if (user == null)
        {
            return NotFound();
        }

        var kweetDtos = user.GetKweets(100, 0);

        return Ok(kweetDtos);
    }

    [HttpPost]
    [Route("post", Name = "PostNewKweet")]
    public async Task<IActionResult> PostKweetAsync(PostKweet command)
    {
        var aggregateId = new UserId(Guid.NewGuid());

        User user = await _userRepo.GetByIdAsync(aggregateId);

        if (user is null)
        {
            return BadRequest();
        }

        try
        {
            user.PostKweet(command);
            await _userRepo.SaveAsync(user);

            return Ok();
        }
        catch (BusinessRuleViolationException e)
        {
            return BadRequest(e.Message);
        }

    }
}
