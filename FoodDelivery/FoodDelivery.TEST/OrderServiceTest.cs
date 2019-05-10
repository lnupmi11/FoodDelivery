using FoodDelivery.BLL.Services;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.DAL.Models;
using FoodDelivery.DAL.Models.Enums;
using FoodDelivery.DTO.Menu;
using FoodDelivery.DTO.Order;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoodDelivery.TEST
{
    class OrderServiceTest
    {
        IUnitOfWork _unitOfWork;
        IList<Order> _orders;
        IList<Address> _addresses;
        IList<OrderItem> _orderItems;
        IList<MenuItem> _menuItems;

        [SetUp]
        public void Setup()
        {
            _addresses = new List<Address>
            {
                new Address{Id="addr1", BuildingNum=1, Region=Region.SumyRegion, Street="str1"},
                new Address{Id="addr2", BuildingNum=2, Region=Region.KyivRegion, Street="str2"},
            };

            _menuItems = new List<MenuItem>
            {
                new MenuItem { Id = "firstMenuItemId", Name = "firstMenuItemName", Description = "firstMenuItemDescription", Price = 100 },
                new MenuItem { Id = "secondMenuItemId", Name = "secondMenuItemName", Description = "secondMenuItemDescription", Price = 200 },
                new MenuItem { Id = "thirdMenuItemId", Name = "thirdMenuItemName", Description = "thirdMenuItemDescription", Price = 300 }
            };

            _orders = new List<Order>
            {
                new Order{ OrderId = "firstId", Address = _addresses[0], SentTime = DateTime.Now.Subtract(new TimeSpan(10000)),
                    EstimatedTime = new TimeSpan(0,1,0,0,0), PaymentType=PaymentType.Cash, Status = OrderStatus.WaitingResponse.ToString() },
                new Order{ OrderId = "secondId", Address = _addresses[1], SentTime = DateTime.Now.Subtract(new TimeSpan(200000)),
                    EstimatedTime = new TimeSpan(0,1,30,0,0), PaymentType=PaymentType.DebitCard, Status = OrderStatus.WaitingResponse.ToString() },
            };

            _orderItems = new List<OrderItem>
            {
                new OrderItem{ OrderItemId="1", MenuItemId="firstMenuItemId", MenuItem=_menuItems[0], Order = _orders[0], Count=2 },
                new OrderItem{ OrderItemId="2", MenuItemId="secondMenuItemId", MenuItem=_menuItems[1], Order = _orders[0], Count=3 },
                new OrderItem{ OrderItemId="3", MenuItemId="thirdMenuItemId", MenuItem=_menuItems[2], Order = _orders[1],Count=1 },
            };

            var orderRepositoryMock = new Mock<IRepository<Order>>();
            orderRepositoryMock.Setup(repository => repository.GetQuery()).Returns(_orders.AsQueryable());
            orderRepositoryMock.Setup(repository => repository.Get(It.IsAny<string>())).Returns((string orderId) => _orders.FirstOrDefault(i => i.OrderId == orderId));
            orderRepositoryMock.Setup(repository => repository.Create(It.IsAny<Order>())).Callback((Order o) => _orders.Add(o));
            orderRepositoryMock.Setup(repository => repository.Update(It.IsAny<Order>())).Callback((Order o) => _orders[_orders.ToList().FindIndex(i => i.OrderId == o.OrderId)] = o);
            orderRepositoryMock.Setup(repository => repository.Delete(It.IsAny<string>())).Callback((string id) => _orders.Remove(_orders.FirstOrDefault(i => i.OrderId == id)));

            var orderItemsRepositoryMock = new Mock<IRepository<OrderItem>>();
            orderItemsRepositoryMock.Setup(repository => repository.GetQuery()).Returns(_orderItems.AsQueryable());

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(ufw => ufw.OrdersRepository).Returns(orderRepositoryMock.Object);
            unitOfWorkMock.Setup(ufw => ufw.OrderItemsRepository).Returns(orderItemsRepositoryMock.Object);

            _unitOfWork = unitOfWorkMock.Object;
        }

        [Test]
        public void GetOrderTest()
        {
            string getId = "firstId";
            var order = _orders.AsQueryable();
            var expectedOrder = _orders.FirstOrDefault(i => i.OrderId == getId);

            var orderService = new OrderService(_unitOfWork);
            var actualItem = orderService.Get(getId);


            Assert.AreEqual(expectedOrder.OrderId, actualItem.OrderId);
        }

        [Test]
        public void GetAllOrdersTest()
        {
            var orders = _orders.AsQueryable();
            var expectedCount = orders.Count();

            var orderService = new OrderService(_unitOfWork);
            var actualCount = orderService.GetAll().Count();

            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        public void AddOrderTest()
        {
            var newOrder = new OrderDTO()
            {
                OrderId = "thirtId",
                EstimatedTime = new TimeSpan(1, 20, 0),
                OrderStatus = OrderStatus.WaitingResponse,
                TotalPrice =1000
            };

            int startCount = _unitOfWork.OrdersRepository.GetQuery().Count();

            OrderService orderService = new OrderService(_unitOfWork);
            orderService.Add(newOrder);

            int finalCount = _unitOfWork.OrdersRepository.GetQuery().Count();
            Assert.AreEqual(startCount + 1, finalCount);

            Order order = _unitOfWork.OrdersRepository.Get(newOrder.OrderId);
            Assert.AreEqual(order.OrderId, newOrder.OrderId);
        }

        [Test]
        public void UpdateOrderTest()
        {
            var orderService = new OrderService(_unitOfWork);
            string toUpdateId = "secondId";
            var toUpdate = orderService.Get(toUpdateId);
            toUpdate.OrderStatus = OrderStatus.Unexpected;
            orderService.Update(toUpdate);
            var actualItem = orderService.Get(toUpdateId);

            Assert.AreEqual(toUpdate.OrderStatus, actualItem.OrderStatus);
        }

        [Test]
        public void DeleteOrderTest()
        {
            var orders = _orders.AsQueryable();
            var expectedCount = orders.Count() - 1;

            var orderService = new OrderService(_unitOfWork);
            var toRemoveId = "secondId";
            var toRemove = orderService.Get(toRemoveId);
            orderService.Delete(toRemove);
            var actualCount = orderService.GetAll().Count();

            Assert.AreEqual(expectedCount, actualCount);
        }
    }
}
