using Ky.Web.CMS.SharedLibarary.Infrastructure.Requests.Admin;
using Microsoft.EntityFrameworkCore;
using Tarumt.CC.Ecommerce.Infrastructure.Context;
using Tarumt.CC.Ecommerce.Infrastructure.Models;

namespace Tarumt.CC.Ecommerce.Services
{
    public class ServerSettingService
    {
        private readonly ILogger<ServerSettingService> _logger;
        private readonly CoreContext _context;

        public ServerSettingService(ILogger<ServerSettingService> logger, CoreContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<ServerSetting> GetAsync()
        {
            _logger.LogInformation("[SERVER SETTING GET]");

            return await _context.ServerSettings
                .Include(m => m.UserServerSettings)
                .Include(m => m.UserPortalServerSettings)
                .FirstAsync() ?? throw new InvalidOperationException("Server Setting not found");
        }

        public async Task<bool> UpdateAsync(ServerSettingAdminRequest serverSettingAdminRequest)
        {
            ServerSetting serverSetting = await GetAsync();

            serverSetting.UserServerSettings = serverSettingAdminRequest.UserServerSettings;
            serverSetting.UserPortalServerSettings = serverSettingAdminRequest.UserPortalServerSettings;

            _context.ServerSettings.Update(serverSetting);
            return await _context.SaveChangesAsync() != 0;
        }
    }
}
