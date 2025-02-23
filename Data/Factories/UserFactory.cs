using Data.Entities;

namespace Data.Factories;

public static class UserFactory
{
    public static UserEntity Create(string firstName, string lastName, string email)
    {
        return new UserEntity
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email
        };
    }
}
