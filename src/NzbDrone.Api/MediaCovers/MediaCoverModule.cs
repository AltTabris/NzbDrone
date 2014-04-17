﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Nancy;
using Nancy.Responses;
using NzbDrone.Common;
using NzbDrone.Common.Disk;
using NzbDrone.Common.EnvironmentInfo;

namespace NzbDrone.Api.MediaCovers
{
    public class MediaCoverModule : NzbDroneApiModule
    {
        private const string MEDIA_COVER_ROUTE = @"/(?<seriesId>\d+)/(?<filename>(.+)\.(jpg|png|gif))";

        private readonly IAppFolderInfo _appFolderInfo;
        private readonly IDiskProvider _diskProvider;

        public MediaCoverModule(IAppFolderInfo appFolderInfo, IDiskProvider diskProvider) : base("MediaCover")
        {
            _appFolderInfo = appFolderInfo;
            _diskProvider = diskProvider;

            Get[MEDIA_COVER_ROUTE] = options => GetMediaCover(options.seriesId, options.filename);
        }

        private Response GetMediaCover(int seriesId, string filename)
        {
            var filePath = Path.Combine(_appFolderInfo.GetAppDataPath(), "MediaCover", seriesId.ToString(), filename);

            if (!_diskProvider.FileExists(filePath))
                return new NotFoundResponse();

            return new StreamResponse(() => File.OpenRead(filePath), MimeTypes.GetMimeType(filePath));
        }
    }
}
