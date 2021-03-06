﻿using LinkfireTechChallenge.Core.Models.Domain;
using System.Threading.Tasks;

namespace LinkfireTechChallenge.Core.Services
{
    public interface IArtistService
    {
        Task<ArtistTopTracks> GetTopTracks(string artistId);
    }
}
