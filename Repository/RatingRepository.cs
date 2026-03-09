using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class RatingRepository : IRatingRepository
    {

        private readonly WebApiShop216328971Context _shopContext;
        public RatingRepository(WebApiShop216328971Context context)
        {
            _shopContext = context;
        }



        public async Task<Rating> AddRating(Rating newRating)
        {
            await _shopContext.Ratings.AddAsync(newRating);
            await _shopContext.SaveChangesAsync();
            return newRating;
        }
    }
}
