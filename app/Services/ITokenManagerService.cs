using System.Threading.Tasks;

namespace SignalRTest.Services
{
    public interface ITokenManagerService
    {
        Task<string> GenerateToken(string username, string password);
        string RefreshToken(string token);
    }
}
