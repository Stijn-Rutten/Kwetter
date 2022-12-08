namespace Kwetter.Api.Dtos
{
    public record KweetDto(string Content, UserId AuthorId, DateTimeOffset PostedAt)
    {
        public static KweetDto MapFrom(Kweet kweet)
        {
            return new KweetDto(kweet.Content!, kweet.AuthorId!, kweet.PostedAt);
        }
    }
}
