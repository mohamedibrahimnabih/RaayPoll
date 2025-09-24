using Mapster;

namespace RaayPoll.API.Mappings
{
    public static class MappingConfig
    {
        public static void RegisterMapsterConfiguration(this IServiceCollection services)
        {
            //TypeAdapterConfig<PollRequest, Poll>
            //    .NewConfig()
            //    .Map(e => e.Description, s => s.Note);
        }
    }
}
