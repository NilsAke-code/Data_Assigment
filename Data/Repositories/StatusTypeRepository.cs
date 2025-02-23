using Data.Contexts;
using Data.Entities;

namespace Data.Repositories;

public class StatusTypeRepository(DataContext context) : BaseRepository<StatusTypeEntity>(context)
{
}
