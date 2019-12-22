using FluentAssertions;
using NUnit.Framework;
using NzbDrone.Core.DecisionEngine.Specifications;
using NzbDrone.Core.Parser.Model;

using NzbDrone.Core.Test.Framework;
using NzbDrone.Core.Indexers;

namespace NzbDrone.Core.Test.DecisionEngineTests
{
    [TestFixture]

    public class RawDiskSpecificationFixture : CoreTest<RawDiskSpecification>
    {
        private RemoteMovie _remoteMovie;

        [SetUp]
        public void Setup()
        {
            _remoteMovie = new RemoteMovie
            {
                Release = new ReleaseInfo
                          {
                              Title = "Movie.title.1998",
                              DownloadProtocol = DownloadProtocol.Torrent
                          }
            };
        }

        private void WithContainer(string container)
        {
            _remoteMovie.Release.Container = container;
        }

        [Test]
        public void should_return_true_if_no_container_specified()
        {
            Subject.IsSatisfiedBy(_remoteMovie, null).Accepted.Should().BeTrue();
        }

        [Test]
        public void should_return_true_if_mkv()
        {
            WithContainer("MKV");
            Subject.IsSatisfiedBy(_remoteMovie, null).Accepted.Should().BeTrue();
        }

        [Test]
        public void should_return_false_if_vob()
        {
            WithContainer("VOB");
            Subject.IsSatisfiedBy(_remoteMovie, null).Accepted.Should().BeFalse();
        }

        [Test]
        public void should_return_false_if_iso()
        {
            WithContainer("ISO");
            Subject.IsSatisfiedBy(_remoteMovie, null).Accepted.Should().BeFalse();
        }

        [Test]
        public void should_return_false_if_m2ts()
        {
            WithContainer("M2TS");
            Subject.IsSatisfiedBy(_remoteMovie, null).Accepted.Should().BeFalse();
        }

        [Test]
        public void should_compare_case_insensitive()
        {
            WithContainer("vob");
            Subject.IsSatisfiedBy(_remoteMovie, null).Accepted.Should().BeFalse();
        }

        [TestCase("How the Earth Was Made S02 Disc 1 1080i Blu-ray DTS-HD MA 2.0 AVC-TrollHD")]
        [TestCase("The Universe S03 Disc 1 1080p Blu-ray LPCM 2.0 AVC-TrollHD")]
        [TestCase("HELL ON WHEELS S02 1080P FULL BLURAY AVC DTS-HD MA 5 1")]
        [TestCase("Game.of.Thrones.S06.2016.DISC.3.BluRay.1080p.AVC.Atmos.TrueHD7.1-MTeam")]
        [TestCase("Game of Thrones S05 Disc 1 BluRay 1080p AVC Atmos TrueHD 7 1-MTeam")]
        public void should_return_false_if_matches_disc_format(string title)
        {
            _remoteMovie.Release.Title = title;
            Subject.IsSatisfiedBy(_remoteMovie, null).Accepted.Should().BeFalse();
        }
    }
}
