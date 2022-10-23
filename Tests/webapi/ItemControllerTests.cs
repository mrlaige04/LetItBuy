using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Moq;
using Newtonsoft.Json;
using NUnit;
using Shop.DAL.Data.EF;
using Shop.DAL.Data.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Shop.Tests.webapi
{

    public class ItemControllerTests
    {
        private readonly Faker<Item> _itemFaker;
        private HttpClient _client;
        private string baseUrl = "https://localhost:7102/api/items";
        [SetUp]
        public void Setup()
        {
            _client = new HttpClient();

            var itemFaker = new Faker<Item>()
                .RuleFor(x => x.IsNew, f => f.Random.Bool())
                .RuleFor(x => x.Name, f => f.Commerce.ProductName())
                .RuleFor(x => x.Price, f => f.Random.Decimal(0, 1000))
                .RuleFor(x => x.Description, f => f.Lorem.Sentence())
                .RuleFor(x => x.ImageUrl, f => f.Image.PicsumUrl())
                .RuleFor(x => x.ID, f => f.Random.Guid());

            var items = itemFaker.Generate(10);
        }

        //////////////////////////////////////////////////
        [Test]
        public async Task GetSuccessIfDBIsNotNull()
        {
            var guid = new Guid();
            var response = await _client.GetStringAsync($"{baseUrl}/{guid}");
            Item item = JsonConvert.DeserializeObject<Item>(response);
            Assert.IsNotNull(item);
        }
        
        [Test]
        public async Task GetNotFound()
        {
            var guidFake = new Faker().Random.Guid();

            var response = await _client.GetAsync($"{baseUrl}/{guidFake}");
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
        [Test]
        public async Task GetInvalidID()
        {
            var invalidGuid = "abc";

            var response = await _client.GetAsync($"{baseUrl}/{invalidGuid}");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        
        //////////////////////////////////////////////////



        
        //////////////////////////////////////////////////
        

        [Test]
        public async Task CreateSuccess()
        {
            var userID = new Guid();
            Item item = new Item();

            //_client.DefaultRequestHeaders.Authorization = JwtHeader.
            //_client.DefaultRequestHeaders.Authorization.Scheme = JwtHeaderParameterNames


        }
        [Test]
        public void CreateUnauthorized()
        {
            
        }

        [Test]
        public void CreateBadRequestModel()
        {

        }

        [Test]
        public void CreateAddingErrorStatusCode()
        {

        }


        
        /////////////////////////////////////////////////
        
        

        [Test]
        public void EditSuccess()
        {

        }
        [Test]
        public void EditUnauthorized()
        {

        }
        [Test]
        public void EditInvalidID()
        {

        }
        [Test]
        public void EditInvalidItem()
        {

        }
        [Test]
        public void EditServerError()
        {

        }

        
        
        //////////////////////////////////////////////////
       


        [Test]
        public void DeleteSuccess()
        {

        }
        [Test]
        public void DeleteInvalidID()
        {

        }
        [Test]
        public void DeleteUnauthorized()
        {
            
        }
        [Test]
        public void DeleteServerError()
        {
            
        }
    }
}
