using NLog;
using NzbDrone.Common;
using NzbDrone.Common.Disk;
using NzbDrone.Common.EnvironmentInfo;

namespace NzbDrone.Update.UpdateEngine
{
    public interface IBackupAndRestore
    {
        void Backup(string source);
        void Restore(string target);
    }

    public class BackupAndRestore : IBackupAndRestore
    {
        private readonly IDiskProvider _diskProvider;
        private readonly IAppFolderInfo _appFolderInfo;
        private readonly Logger _logger;

        public BackupAndRestore(IDiskProvider diskProvider, IAppFolderInfo appFolderInfo, Logger logger)
        {
            _diskProvider = diskProvider;
            _appFolderInfo = appFolderInfo;
            _logger = logger;
        }

        public void Backup(string source)
        {
            _logger.Info("Creating backup of existing installation");
            _diskProvider.CopyFolder(source, _appFolderInfo.GetUpdateBackUpFolder());
        }

        public void Restore(string target)
        {
            _logger.Info("Attempting to rollback upgrade");
            _diskProvider.CopyFolder(_appFolderInfo.GetUpdateBackUpFolder(), target);
        }
    }
}