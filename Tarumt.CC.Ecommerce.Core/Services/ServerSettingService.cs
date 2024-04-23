using Microsoft.EntityFrameworkCore;
using Tarumt.CC.Ecommerce.Core.Infrastructure.Context;
using Tarumt.CC.Ecommerce.Core.Infrastructure.Models;
using Tarumt.CC.Ecommerce.SharedLibrary.Infrastructure.Requests.Admin;

namespace Tarumt.CC.Ecommerce.Core.Services
{
    public class ServerSettingService(ILogger<ServerSettingService> logger, CoreContext context)
    {
        public async Task<ServerSetting> GetAsync()
        {
            logger.LogInformation("[SERVER SETTING GET]");

            return await context.ServerSettings
                .Include(m => m.UserServerSettings)
                .Include(m => m.UserPortalServerSettings)
                .FirstAsync() ?? throw new InvalidOperationException("Server Setting not found");
        }

        public async Task<bool> UpdateAsync(ServerSettingAdminRequest serverSettingAdminRequest)
        {
            ServerSetting serverSetting = await GetAsync();

            serverSetting.UserServerSettings = serverSettingAdminRequest.UserServerSettings;
            serverSetting.UserPortalServerSettings = serverSettingAdminRequest.UserPortalServerSettings;

            context.ServerSettings.Update(serverSetting);
            return await context.SaveChangesAsync() != 0;
        }
    }
}
