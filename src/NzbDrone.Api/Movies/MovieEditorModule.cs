using System;
using System.Collections.Generic;
using System.Linq;
using Nancy;
using Nancy.Responses;
using Radarr.Http.Extensions;
using Radarr.Http.REST;
using NzbDrone.Core.Movies;
using Radarr.Http.Mapping;

namespace NzbDrone.Api.Movies
{
    public class MovieEditorModule : NzbDroneApiModule
    {
        private readonly IMovieService _movieService;

        public MovieEditorModule(IMovieService movieService)
            : base("/movie/editor")
        {
            _movieService = movieService;
            Put("/",  Movie => SaveAll());
            Put("/delete",  Movie => DeleteSelected());
        }

        private object SaveAll()
        {
            var resources = Request.Body.FromJson<List<MovieResource>>();

            var Movie = resources.Select(MovieResource => MovieResource.ToModel(_movieService.GetMovie(MovieResource.Id))).ToList();

            return ResponseWithCode(_movieService.UpdateMovie(Movie)
                                 .ToResource()
                                 , HttpStatusCode.Accepted);
        }

        private object DeleteSelected()
        {
            var deleteFiles = false;
            var addExclusion = false;
            var deleteFilesQuery = Request.Query.deleteFiles;
            var addExclusionQuery = Request.Query.addExclusion;

            if (deleteFilesQuery.HasValue)
            {
                deleteFiles = Convert.ToBoolean(deleteFilesQuery.Value);
            }
            if (addExclusionQuery.HasValue)
            {
                addExclusion = Convert.ToBoolean(addExclusionQuery.Value);
            }
            var ids = Request.Body.FromJson<List<int>>();

            foreach (var id in ids)
            {
                _movieService.DeleteMovie(id, deleteFiles, addExclusion);
            }

            return new Response
            {
                StatusCode = HttpStatusCode.Accepted
            };
        }
    }
}
