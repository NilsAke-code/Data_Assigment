using Data.Contexts;
using Data.Entities;

namespace Data.Repositories;

public class ServiceRepository(DataContext context) : BaseRepository<ServiceEntity>(context)
{
}
