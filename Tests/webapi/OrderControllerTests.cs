
using Bogus;
using Shop.DAL.Data.Entities;

namespace Shop.Tests.webapi
{
    public class OrderControllerTests
    {
        private Faker<Order> _orderFaker;
        private Faker<DeliveryInfo> _deliveryInfoFaker;
        [SetUp]
        public void Setup()
        {
            _deliveryInfoFaker = new Faker<DeliveryInfo>()
                .RuleFor(x => x.DeliveryID, f => f.Random.Guid())
                .RuleFor(x => x.Name, f => f.Person.FirstName)
                .RuleFor(x => x.LastName, f => f.Person.LastName)
                .RuleFor(x => x.City, f => f.Address.City())
                .RuleFor(x => x.PostName, f => f.Commerce.ProductName())
                .RuleFor(x => x.PostNumber, f => f.Address.BuildingNumber())
                .RuleFor(x => x.PostAddress, f => f.Address.StreetAddress())
                .RuleFor(x => x.TrackNumber, f => f.Random.String());

            _orderFaker = new Faker<Order>()
                .RuleFor(x => x.OrderID, f => f.Random.Guid())
                .RuleFor(x => x.ItemName, f => f.Commerce.ProductName())
                .RuleFor(x => x.ItemID, f => f.Random.Guid())
                .RuleFor(x => x.DeliveryInfo, f => _deliveryInfoFaker.Generate())
                .RuleFor(x => x.Status, f => (OrderStatus)f.Random.Int(0, 5));
        }

        [Test]
        public void CreateUnauthorized()
        {
            /// not insert auth headers
            Assert.Pass();
        }
        [Test]
        public void CreateInvalidModel()
        {
            //Make invalid model
            Assert.Pass();
        }
        [Test]
        public void CreateSuccess()
        {
            /// Take first and second user from db and make faker with that ids
            /// and call api
            Assert.Pass();
        }

        [Test]
        public void CreateNotExistsOwner()
        {
            /// Take exists user and make him BUYER
            /// and call api
            Assert.Pass();
        }

        [Test]
        public void CreateNotExistsBuyerIfAuthorized()
        {
            /// Take exists user and make him OWNER
            /// and call api
            Assert.Pass();
        }

        [Test]
        public void CreateInternalError()
        {
            /// IDK HOW TO DO IT))))))))))))
            /// MAYBE INVALID ID
            Assert.Pass();
        }


        /////////////////////////////////////////////////




        [Test]
        public void EditInvalidModel()
        {
            /// Invalid model
            Assert.Pass();
        }

        [Test]
        public void EditBuyerNotExists()
        {
            Assert.Pass();
        }

        [Test]
        public void EditOwnerNotExists()
        {
            Assert.Pass();
        }

        [Test]
        public void EditInternalError()
        {
            Assert.Pass();
        }

        [Test]
        public void EditNotFound()
        {
            Assert.Pass();
        }
        [Test]
        public void EditSuccess()
        {
            Assert.Pass();
        }
        [Test]
        public void EditUnauthorized()
        {
            Assert.Pass();
        }

        /////////////////////////////////////////////

        [Test]
        public void DeleteInvalidModel()
        {
            //Invalid GUID
            Assert.Pass();
        }
        [Test]
        public void DeleteNotParsableGuid()
        {
            Assert.Pass();
        }

        [Test]
        public void DeleteNotFound()
        {
            Assert.Pass();
        }

        [Test]
        public void DeleteNotBuyerAuthorized()
        {
            Assert.Pass();
        }

        [Test]
        public void DeleteNotOwnerAuthorized()
        {
            Assert.Pass();
        }

        [Test]
        public void DeleteUnauthorized()
        {
            Assert.Pass();
        }

        [Test]
        public void DeleteInternalError()
        {
            Assert.Pass();
        }
        [Test]
        public void DeleteSuccess()
        {
            Assert.Pass();
        }

        //////////////////////////////////


        [Test]
        public void CancelUnauthorized()
        {
            Assert.Pass();
        }

        [Test]
        public void CancelInvalidModel()
        {
            Assert.Pass();
        }

        [Test]
        public void CancelNotParsableGuid()
        {
            Assert.Pass();
        }

        [Test]
        public void CancelNotOwnerAuthorized()
        {
            Assert.Pass();
        }

        [Test]
        public void CancelNotBuyerAuthorized()
        {
            Assert.Pass();
        }

        [Test]
        public void CancelNotFound()
        {
            Assert.Pass();
        }

        [Test]
        public void CancelInternalError()
        {
            Assert.Pass();
        }
        [Test]
        public void CancelSuccess()
        {
            Assert.Pass();
        }

        //////////////////////////



        [Test]
        public void DeliveryInvalidID()
        {
            Assert.Pass();
        }

        [Test]
        public void DeliveryInvalidTrackNumber()
        {
            Assert.Pass();
        }

        [Test]
        public void DeliveryOrderNotFound()
        {
            Assert.Pass();
        }

        [Test]
        public void DeliveryDelInfoNotFound()
        {
            Assert.Pass();
        }

        [Test]
        public void DeliveryUnauthorized()
        {
            Assert.Pass();
        }

        [Test]
        public void DeliveryNotOwnerAuthorized()
        {
            Assert.Pass();
        }

        [Test]
        public void DeliveryInternalError()
        {
            Assert.Pass();
        }

        [Test]
        public void DeliverySuccess()
        {
            Assert.Pass();
        }
    }
}
