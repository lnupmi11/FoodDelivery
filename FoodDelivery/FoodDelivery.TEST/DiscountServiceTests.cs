using FoodDelivery.BLL.Services;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.DAL.Models;
using FoodDelivery.DAL.Models.Enums;
using FoodDelivery.DTO.Menu;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoodDelivery.TEST
{
    class DiscountServiceTest
    {
        IUnitOfWork _unitOfWork;
        IList<Discount> _discounts;

        [SetUp]
        public void Setup()
        {
            _discounts = new List<Discount>
            {
                new Discount{Id = "firstId", Description="Description1", Percentage=1},
                new Discount{Id = "secondId", Description="Description2", Percentage=2},
                new Discount{Id = "thirdId", Description="Description3", Percentage=3},
            };

            var discountRepositoryMock = new Mock<IRepository<Discount>>();
            discountRepositoryMock.Setup(repository => repository.GetQuery()).Returns(_discounts.AsQueryable());
            discountRepositoryMock.Setup(repository => repository.Get(It.IsAny<string>())).Returns((string id) => _discounts.FirstOrDefault(i => i.Id == id));
            discountRepositoryMock.Setup(repository => repository.Create(It.IsAny<Discount>())).Callback((Discount o) => _discounts.Add(o));
            discountRepositoryMock.Setup(repository => repository.Update(It.IsAny<Discount>())).Callback((Discount o) => _discounts[_discounts.ToList().FindIndex(i => i.Id == o.Id)] = o);
            discountRepositoryMock.Setup(repository => repository.Delete(It.IsAny<string>())).Callback((string id) => _discounts.Remove(_discounts.FirstOrDefault(i => i.Id == id)));

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(ufw => ufw.DiscountsRepository).Returns(discountRepositoryMock.Object);

            _unitOfWork = unitOfWorkMock.Object;
        }

        [Test]
        public void GetdiscountTest()
        {
            string getId = "firstId";
            var discount = _discounts.AsQueryable();
            var expecteddiscount = _discounts.FirstOrDefault(i => i.Id == getId);

            var DiscountService = new DiscountService(_unitOfWork);
            var actualItem = DiscountService.Get(getId);


            Assert.AreEqual(expecteddiscount.Id, actualItem.Id);
        }

        [Test]
        public void GetAllDiscountsTest()
        {
            var discounts = _discounts.AsQueryable();
            var expectedCount = discounts.Count();

            var DiscountService = new DiscountService(_unitOfWork);
            var actualCount = DiscountService.GetAll().Count();

            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        public void AdddiscountTest()
        {
            var newdiscount = new DiscountDTO()
            {
                Id="fourthId",
                Description="Description3",
                Percentage=3
            };

            int startCount = _unitOfWork.DiscountsRepository.GetQuery().Count();

            DiscountService DiscountService = new DiscountService(_unitOfWork);
            DiscountService.Add(newdiscount);

            int finalCount = _unitOfWork.DiscountsRepository.GetQuery().Count();
            Assert.AreEqual(startCount + 1, finalCount);

            Discount discount = _unitOfWork.DiscountsRepository.Get(newdiscount.Id);
            Assert.AreEqual(discount.Id, newdiscount.Id);
        }

        [Test]
        public void UpdatediscountTest()
        {
            var DiscountService = new DiscountService(_unitOfWork);
            string toUpdateId = "secondId";
            var toUpdate = DiscountService.Get(toUpdateId);
            toUpdate.Percentage = 20;
            DiscountService.Update(toUpdate);
            var actualItem = DiscountService.Get(toUpdateId);

            Assert.AreEqual(toUpdate.Percentage, actualItem.Percentage);
        }

        [Test]
        public void DeletediscountTest()
        {
            var discounts = _discounts.AsQueryable();
            var expectedCount = discounts.Count() - 1;

            var DiscountService = new DiscountService(_unitOfWork);
            var toRemoveId = "secondId";
            var toRemove = DiscountService.Get(toRemoveId);
            DiscountService.Delete(toRemove);
            var actualCount = DiscountService.GetAll().Count();

            Assert.AreEqual(expectedCount, actualCount);
        }
    }
}
