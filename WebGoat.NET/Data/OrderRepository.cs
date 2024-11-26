using WebGoatCore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Dapper;
using System.Data.SQLite;
using System.IO;
using Microsoft.Data.Sqlite;
using System.Configuration;
using System.Data.Common;
using System.Data;

namespace WebGoatCore.Data
{
    public class OrderRepository
    {
        private readonly NorthwindContext _context;
        private readonly CustomerRepository _customerRepository;

        public OrderRepository(NorthwindContext context, CustomerRepository customerRepository)
        {
            _context = context;
            _customerRepository = customerRepository;
        }

        public Order GetOrderById(int orderId)
        {
            return _context.Orders.Single(o => o.OrderId == orderId);
        }

        public int CreateOrder(Order order)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            // These commented lines cause EF Core to do wierd things.
            // Instead, make the query manually.

            // order = _context.Orders.Add(order).Entity;
            // _context.SaveChanges();
            // return order.OrderId;


            // Solution using Dapper
            // https://www.learndapper.com/saving-data/insert
            // https://www.learndapper.com/saving-data/insert#dapper-insert-multiple-rows (for orderDetails)
            // https://stackoverflow.com/questions/17150542/how-to-insert-a-c-sharp-list-to-database-using-dapper-net (for orderDetails)
            // https://www.learndapper.com/parameters
            // https://www.learndapper.com/misc/transaction
            // https://www.sqlite.org/c3ref/last_insert_rowid.html

            using (var connection = NorthwindContext.GetDapperConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int insertedOrderId = InsertOrder(connection, transaction, order);

                        InsertOrderDetails(connection, transaction, order.OrderDetails, insertedOrderId);

                        InsertShipment(connection, transaction, order.Shipment!, insertedOrderId);

                        transaction.Commit();

                        return insertedOrderId;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        public void UpdateOrder(Order order)
        {
            // Attach the order to the DbContext if it's not already tracked
            var existingOrder = _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefault(o => o.OrderId == order.OrderId);

            if (existingOrder == null)
            {
                throw new ArgumentException($"Order with ID {order.OrderId} does not exist.");
            }

            // Update scalar properties
            existingOrder.OrderDate = order.OrderDate;
            existingOrder.RequiredDate = order.RequiredDate;
            existingOrder.ShippedDate = order.ShippedDate;
            existingOrder.Freight = order.Freight;
            existingOrder.ShipName = order.ShipName;
            existingOrder.ShipAddress = order.ShipAddress;
            existingOrder.ShipCity = order.ShipCity;
            existingOrder.ShipRegion = order.ShipRegion;
            existingOrder.ShipPostalCode = order.ShipPostalCode;
            existingOrder.ShipCountry = order.ShipCountry;

            // Update related OrderDetails
            foreach (var detail in order.OrderDetails)
            {
                var existingDetail = existingOrder.OrderDetails
                    .FirstOrDefault(od => od.OrderId == detail.OrderId);

                if (existingDetail != null)
                {
                    // Update existing detail
                    existingDetail.Quantity = detail.Quantity;
                    existingDetail.Discount = detail.Discount;
                }
                else
                {
                    // Add new detail
                    existingOrder.OrderDetails.Add(detail);
                }
            }

            // Remove details no longer in the updated order
            foreach (var existingDetail in existingOrder.OrderDetails.ToList())
            {
                if (!order.OrderDetails.Any(od => od.OrderId == existingDetail.OrderId))
                {
                    _context.OrderDetails.Remove(existingDetail);
                }
            }

            // Save changes to the database
            _context.SaveChanges();
        }


        public void CreateOrderPayment(int orderId, decimal amountPaid, string creditCardNumber, DateTime expirationDate, string approvalCode)
        {
            var orderPayment = new OrderPayment()
            {
                AmountPaid = Convert.ToDouble(amountPaid),
                CreditCardNumber = creditCardNumber,
                ApprovalCode = approvalCode,
                ExpirationDate = expirationDate,
                OrderId = orderId,
                PaymentDate = DateTime.Now
            };
            _context.OrderPayments.Add(orderPayment);
            _context.SaveChanges();
        }
        public List<Order> GetAllOrders()
        {
            return _context.Orders.OrderByDescending(p => p.OrderDate).ToList();
        }

        //public IActionResult Index(string SearchString)
        //{
        //    // Retrieve orders based on the search string
        //    var orders = _context.Orders
        //                         .Where(o => o.OrderId.ToString().Contains(SearchString))
        //                         .ToList();

        //    // Pass the filtered orders to the view
        //    return _context.Orders(orders);
        //}
        public ICollection<Order> GetAllOrdersByCustomerId(string customerId)
        {
            return _context.Orders
                .Where(o => o.CustomerId == customerId)
                .OrderByDescending(o => o.OrderDate)
                .ThenByDescending(o => o.OrderId)
                .ToList();
        }

        private int InsertOrder(IDbConnection connection, IDbTransaction transaction, Order order)
        {
            string sql = @"INSERT INTO Orders (CustomerId, EmployeeId, OrderDate, RequiredDate, ShippedDate, ShipVia, Freight, ShipName, ShipAddress, ShipCity, ShipRegion, ShipPostalCode, ShipCountry)
                                              VALUES (@CustomerId, @EmployeeId, @OrderDate, @RequiredDate, @ShippedDate, @ShipVia, @Freight, @ShipName, @ShipAddress, @ShipCity, @ShipRegion, @ShipPostalCode, @ShipCountry);
                                              SELECT last_insert_rowid();";

            return connection.QuerySingle<int>(sql, new
            {
                CustomerId = order.CustomerId,
                EmployeeId = order.EmployeeId,
                OrderDate = order.OrderDate.ToString("yyyy-MM-dd"),
                RequiredDate = order.RequiredDate.ToString("yyyy-MM-dd"),
                ShippedDate = order.ShippedDate.HasValue ? order.ShippedDate?.ToString("yyyy-MM-dd") : null,
                ShipVia = order.ShipVia,
                Freight = order.Freight,
                ShipName = order.ShipName,
                ShipAddress = order.ShipAddress,
                ShipCity = order.ShipCity,
                ShipRegion = order.ShipRegion,
                ShipPostalCode = order.ShipPostalCode,
                ShipCountry = order.ShipCountry
            }, transaction);
        }

        private void InsertOrderDetails(IDbConnection connection, IDbTransaction transaction, IEnumerable<OrderDetail> orderDetails, int orderId)
        {
            if (orderDetails != null && orderDetails.Any())
            {
                foreach (var orderDetail in orderDetails)
                {
                    orderDetail.OrderId = orderId;
                }

                string sql = @"INSERT INTO OrderDetails (OrderId, ProductId, UnitPrice, Quantity, Discount)
                                                        VALUES(@OrderId, @ProductId, @UnitPrice, @Quantity, @Discount)";

                int rowsAffected = connection.Execute(sql, orderDetails, transaction);

                if (rowsAffected == 0)
                    throw new Exception("Something when wrong when trying to insert OrderDetails");
            }
        }

        private void InsertShipment(IDbConnection connection, IDbTransaction transaction, Shipment shipment, int orderId)
        {
            if (shipment != null)
            {
                string sql = @"INSERT INTO Shipments (OrderId, ShipperId, ShipmentDate, TrackingNumber)
                                                    VALUES (@OrderId, @ShipperId, @ShipmentDate, @TrackingNumber)";

                int rowsAffected = connection.Execute(sql, new 
                {
                    OrderId = orderId,
                    ShipperId = shipment.ShipperId,
                    ShipmentDate = shipment.ShipmentDate.ToString("yyyy-MM-dd"),
                    TrackingNumber = shipment.TrackingNumber
                }, transaction);

                if (rowsAffected == 0)
                    throw new Exception("Something when wrong when trying to insert Shipment");
            }
        }
    }
}
