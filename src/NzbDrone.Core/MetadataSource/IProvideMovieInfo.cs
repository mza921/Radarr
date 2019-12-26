using System;
using System.Collections.Generic;
using NzbDrone.Core.Movies;
using NzbDrone.Core.Profiles;

namespace NzbDrone.Core.MetadataSource
{
    public interface IProvideMovieInfo
    {
        Movie GetMovieInfo(string ImdbId);
        Movie GetMovieInfo(int TmdbId, Profile profile, bool hasPreDBEntry);
        HashSet<int> GetChangedMovies(DateTime startTime);
    }
}
