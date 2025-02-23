using Data.Entities;

namespace Data.Factories;

public static class StatusTypeFactory
{
    public static StatusTypeEntity Create(string statusName)
    {
        return new StatusTypeEntity
        {
            StatusName = statusName
        };
    }
}
