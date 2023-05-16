using Application.Common.Mappings;
using Domain.Entities;
using Domain.Enums;

namespace Application.ApiCallJobCommandQuery;

public record ApiCallJobDto : IMapFrom<ApiCallJob>
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Url { get; set; }
    public ApiMethod Method { get; set; }
    public string? JsonBody { get; set; }
    public Dictionary<string, string>? Headers { get; set; }
    public int MonitoringInterval { get; set; }

}
