﻿using System;
using System.Collections.Generic;
using System.IO;
using FluentValidation.Results;
using NLog;
using NzbDrone.Common;
using NzbDrone.Common.Disk;
using NzbDrone.Common.Http;
using NzbDrone.Core.Configuration;
using NzbDrone.Core.Indexers;
using NzbDrone.Core.Organizer;
using NzbDrone.Core.Parser;
using NzbDrone.Core.Parser.Model;

namespace NzbDrone.Core.Download.Clients.Pneumatic
{
    public class Pneumatic : DownloadClientBase<PneumaticSettings>
    {
        private readonly IHttpProvider _httpProvider;

        public Pneumatic(IHttpProvider httpProvider,
                         IConfigService configService,
                         IDiskProvider diskProvider,
                         IParsingService parsingService,
                         Logger logger)
            : base(configService, diskProvider, parsingService, logger)
        {
            _httpProvider = httpProvider;
        }

        public override DownloadProtocol Protocol
        {
            get
            {
                return DownloadProtocol.Usenet;
            }
        }

        public override string Download(RemoteEpisode remoteEpisode)
        {
            var url = remoteEpisode.Release.DownloadUrl;
            var title = remoteEpisode.Release.Title;

            if (remoteEpisode.ParsedEpisodeInfo.FullSeason)
            {
                throw new NotSupportedException("Full season releases are not supported with Pneumatic.");
            }

            title = FileNameBuilder.CleanFileName(title);

            //Save to the Pneumatic directory (The user will need to ensure its accessible by XBMC)
            var filename = Path.Combine(Settings.NzbFolder, title + ".nzb");

            _logger.Debug("Downloading NZB from: {0} to: {1}", url, filename);
            _httpProvider.DownloadFile(url, filename);

            _logger.Debug("NZB Download succeeded, saved to: {0}", filename);

            var contents = String.Format("plugin://plugin.program.pneumatic/?mode=strm&type=add_file&nzb={0}&nzbname={1}", filename, title);
            _diskProvider.WriteAllText(Path.Combine(_configService.DownloadedEpisodesFolder, title + ".strm"), contents);

            return null;
        }

        public bool IsConfigured
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Settings.NzbFolder);
            }
        }

        public override IEnumerable<DownloadClientItem> GetItems()
        {
            return new DownloadClientItem[0];
        }

        public override void RemoveItem(String id)
        {
            throw new NotSupportedException();
        }

        public override String RetryDownload(String id)
        {
            throw new NotSupportedException();
        }

        public override DownloadClientStatus GetStatus()
        {
            var status = new DownloadClientStatus
            {
                IsLocalhost = true
            };

            return status;
        }

        protected override void Test(List<ValidationFailure> failures)
        {
            failures.AddIfNotNull(TestWrite(Settings.NzbFolder, "NzbFolder"));
        }

        private ValidationFailure TestWrite(String folder, String propertyName)
        {
            if (!_diskProvider.FolderExists(folder))
            {
                return new ValidationFailure(propertyName, "Folder does not exist");
            }

            try
            {
                var testPath = Path.Combine(folder, "drone_test.txt");
                _diskProvider.WriteAllText(testPath, DateTime.Now.ToString());
                _diskProvider.DeleteFile(testPath);
            }
            catch (Exception ex)
            {
                _logger.ErrorException(ex.Message, ex);
                return new ValidationFailure(propertyName, "Unable to write to folder");
            }

            return null;
        }
    }
}
