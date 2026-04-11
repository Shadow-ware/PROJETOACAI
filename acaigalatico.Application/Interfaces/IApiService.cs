using System.Collections.Generic;
using System.Threading.Tasks;

namespace acaigalatico.Application.Interfaces
{
    public interface IApiService
    {
        Task<IEnumerable<dynamic>> GetPostsAsync();
        Task<dynamic> CreatePostAsync(object postData);
    }
}
