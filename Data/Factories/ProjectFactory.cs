using Data.Entities;

namespace Data.Factories;

public static class ProjectFactory
{
    public static ProjectEntity Create(string title, string description, int customerId, int serviceId, int userId, int statusId, DateTime startDate, DateTime endDate)
    {
        return new ProjectEntity
        {
            Title = title ?? "Standardtitel",
            Description = description ?? "Ingen beskrivning",
            CustomerId = customerId,
            ServiceId = serviceId,
            UserId = userId,
            StatusId = statusId,
            StartDate = startDate,
            EndDate = endDate
        };
    }
}
