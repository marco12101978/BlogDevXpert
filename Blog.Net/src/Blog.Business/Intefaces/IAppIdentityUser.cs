using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Business.Intefaces
{
    public interface IAppIdentityUser
    {
        string GetUsername();
        Guid GetUserId();
        bool IsAuthenticated();
        bool IsInRole(string role);
        string GetRemoteIpAddress();
        string GetLocalIpAddress();
    }
}
