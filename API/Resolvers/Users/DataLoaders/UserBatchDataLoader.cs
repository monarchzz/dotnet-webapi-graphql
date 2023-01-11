using Domain.Entities;
using EFCore.Repository;

namespace API.Resolvers.Users.DataLoaders;

public class UserBatchDataLoader : BatchDataLoader<Guid, User?>
{
    private readonly IAppRepository<User> _userRepository;

    public UserBatchDataLoader(IBatchScheduler batchScheduler, IAppRepository<User> userRepository,
        DataLoaderOptions? options = null) : base(batchScheduler, options)
    {
        _userRepository = userRepository;
    }

    protected override async Task<IReadOnlyDictionary<Guid, User?>> LoadBatchAsync(IReadOnlyList<Guid> keys,
        CancellationToken cancellationToken)
    {
        var users = await _userRepository.Finds(keys.ToList(), cancellationToken);
        Console.WriteLine(string.Join(",", users.Select(u => u.Id)));

        return keys.ToDictionary(k => k, k => users.FirstOrDefault(u => u.Id == k));
    }
}