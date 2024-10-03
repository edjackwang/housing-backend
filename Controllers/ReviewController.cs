﻿using Microsoft.AspNetCore.Mvc;
using SimpleWebAppReact.Entities;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using SimpleWebAppReact.Services;

namespace SimpleWebAppReact.Controllers
{
    /// <summary>
    /// Defines endpoints for operations relating the Review table
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly ILogger<ReviewController> _logger;
        private readonly IMongoCollection<Review>? _reviews;

        public ReviewController(ILogger<ReviewController> logger, MongoDbService mongoDbService)
        {
            _logger = logger;
            _reviews = mongoDbService.Database?.GetCollection<Review>("review");
        }

        /// <summary>
        /// gets reviews
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<Review>> Get()
        {
            // Fetch the reviews from the database
            var reviews = await _reviews.Find(_ => true).ToListAsync();

            return Ok(reviews);
        }

        /// <summary>
        /// gets specific review with id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Review?>> GetById([FromRoute] string id)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            
            var filter = Builders<Review>.Filter.Eq(x => x.Id, id);
            var review = _reviews.Find(filter).FirstOrDefault();
            
            return review is not null ? Ok(review) : NotFound();
        }

        /// <summary>
        /// adds review entry to table
        /// </summary>
        /// <param name="review"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Post(Review review)
        {
            await _reviews.InsertOneAsync(review);
            return CreatedAtAction(nameof(GetById), new { id = review.Id }, review);
        }

        /// <summary>
        /// updates a review entry
        /// </summary>
        /// <param name="review"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult> Update(Review review)
        {
            var filter = Builders<Review>.Filter.Eq(x => x.Id, review.Id);
            await _reviews.ReplaceOneAsync(filter, review);
            return Ok();
        }

        /// <summary>
        /// deletes a review entry
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var filter = Builders<Review>.Filter.Eq(x => x.Id, id);
            await _reviews.DeleteOneAsync(filter);
            return Ok();
        }
    }
}