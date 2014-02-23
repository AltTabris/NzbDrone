using System;
using System.IO;
using System.Linq;
using FizzWare.NBuilder;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using NzbDrone.Common;
using NzbDrone.Core.Download;
using NzbDrone.Core.Download.Clients.Sabnzbd;
using NzbDrone.Core.Download.Clients.Sabnzbd.Responses;
using NzbDrone.Core.Parser.Model;
using NzbDrone.Core.Test.Framework;
using NzbDrone.Core.Tv;

namespace NzbDrone.Core.Test.Download.DownloadClientTests.SabnzbdTests
{
    [TestFixture]
    public class SabnzbdFixture : CoreTest<Sabnzbd>
    {
        private const string URL = "http://www.nzbclub.com/nzb_download.aspx?mid=1950232";
        private const string TITLE = "My Series Name - 5x2-5x3 - My title [Bluray720p] [Proper]";
        private RemoteEpisode _remoteEpisode;

        [SetUp]
        public void Setup()
        {
            _remoteEpisode = new RemoteEpisode();
            _remoteEpisode.Release = new ReleaseInfo();
            _remoteEpisode.Release.Title = TITLE;
            _remoteEpisode.Release.DownloadUrl = URL;

            _remoteEpisode.Episodes = Builder<Episode>.CreateListOfSize(1)
                                                      .All()
                                                      .With(e => e.AirDate = DateTime.Today.ToString(Episode.AIR_DATE_FORMAT))
                                                      .Build()
                                                      .ToList();

            Subject.Definition = new DownloadClientDefinition();
            Subject.Definition.Settings = new SabnzbdSettings
                                          {
                                              Host = "192.168.5.55",
                                              Port = 2222,
                                              ApiKey = "5c770e3197e4fe763423ee7c392c25d1",
                                              Username = "admin",
                                              Password = "pass",
                                              TvCategory = "tv",
                                              RecentTvPriority = (int)SabnzbdPriority.High
                                          };
        }

        [Test]
        public void downloadNzb_should_use_sabRecentTvPriority_when_recentEpisode_is_true()
        {
            Mocker.GetMock<ISabnzbdProxy>()
                    .Setup(s => s.DownloadNzb(It.IsAny<Stream>(), It.IsAny<String>(), It.IsAny<String>(), (int)SabnzbdPriority.High, It.IsAny<SabnzbdSettings>()))
                    .Returns(new SabnzbdAddResponse());

            Subject.DownloadNzb(_remoteEpisode);

            Mocker.GetMock<ISabnzbdProxy>()
                  .Verify(v => v.DownloadNzb(It.IsAny<Stream>(), It.IsAny<String>(), It.IsAny<String>(), (int)SabnzbdPriority.High, It.IsAny<SabnzbdSettings>()), Times.Once());
        }
    }
}
