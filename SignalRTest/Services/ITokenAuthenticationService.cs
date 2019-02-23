using System.Threading.Tasks;

namespace SignalRTest.Services
{
    public interface ITokenAuthenticationService
    {
        Task<string> Authenticate(string username, string password);
    }
}
