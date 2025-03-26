using ProjectManager.Domain.Models;

namespace AuthenticationService.Services
{
    internal sealed class TokenStore
    {
        private List<AccessToken> _store = new();


        public List<AccessToken> Get() => _store;
        public AccessToken? Get(int userId)
            => _store.Find(x => x.UserId == userId);
        public AccessToken? Get(Guid id)
            => _store.Find(x => x.TokenId == id);
        public void Add(AccessToken token)
            => _store.Add(token);
        public void Remove(AccessToken token)
            => _store.Remove(token);
        public void Remove(Guid id)
            => _store.RemoveAll(x => x.TokenId == id);
    }
}
