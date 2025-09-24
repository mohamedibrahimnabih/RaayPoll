using Azure;

namespace RaayPoll.API.DTOs.Responses
{
    public record PollResponse(
        int Id,
        string Name,
        string? Description);

    //public class PollResponse
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; } = string.Empty;
    //    public string? Description { get; set; }

    //    //public static explicit operator PollResponse(Poll response)
    //    //{
    //    //    return new PollResponse()
    //    //    {
    //    //        Id = response.Id,
    //    //        Name = response.Name,
    //    //        Description = response.Description,
    //    //    };
    //    //}
    //}
}
