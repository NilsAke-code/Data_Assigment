using Data.Entities;
using Data.Interfaces;

namespace Data.Services;

public class UserService(IRepository<UserEntity> userRepository)
{
    private readonly IRepository<UserEntity> _userRepository = userRepository;

    public async Task<bool> CreateUserAsync(UserEntity user)
    {
        if (string.IsNullOrEmpty(user.FirstName) || string.IsNullOrEmpty(user.LastName))
        {
            Console.WriteLine("Fel: Namn får inte vara tomt.");
            return false;
        }
        await _userRepository.AddAsync(user);
        return true;
    }
    public async Task<IEnumerable<UserEntity>> GetAllUserAsync()
    {
        return await _userRepository.GetAllAsync();
    }
    public async Task<UserEntity?> GetUserByIdAsync(int id)
    {
        return await _userRepository.GetByIdAsync(id);
    }
}
