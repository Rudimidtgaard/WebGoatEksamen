using WebGoatCore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Threading;
using System.Data.SqlClient;
using Dapper;
using System.Data.SQLite;
using System.IO;
using Microsoft.Data.Sqlite;
using System.Configuration;

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

                // De 3 sql'er skulle have været pakket ind i en transaction.
                // Men pga. opbygningen af Foreign Key Trigger constraints på OrderDetail
                // tror jeg ikke triggeren kan finde OrderId, fordi den ikke er commited til databasen

                //using (var transaction =  connection.BeginTransaction())

                try
                {
                    string createOrderSql = @"INSERT INTO Orders (CustomerId, EmployeeId, OrderDate, RequiredDate, ShippedDate, ShipVia, Freight, ShipName, ShipAddress, ShipCity, ShipRegion, ShipPostalCode, ShipCountry)
                                                VALUES (@CustomerId, @EmployeeId, @OrderDate, @RequiredDate, @ShippedDate, @ShipVia, @Freight, @ShipName, @ShipAddress, @ShipCity, @ShipRegion, @ShipPostalCode, @ShipCountry);
                                                SELECT OrderId FROM Orders WHERE rowid = last_insert_rowid()"; // could be simplified to select last_insert_rowid(); ?

                    order.OrderId = connection.QuerySingle<int>(createOrderSql, new
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
                    });

                    string createOrderDetailsSql = @"INSERT INTO OrderDetails (OrderId, ProductId, UnitPrice, Quantity, Discount)
                                                        VALUES(@OrderId, @ProductId, @UnitPrice, @Quantity, @Discount)";

                    int orderDetailsRowsAffected = connection.Execute(createOrderDetailsSql, order.OrderDetails); // Check if rowsAffected > 0 

                    string createShipmentSql = @"INSERT INTO Shipments (OrderId, ShipperId, ShipmentDate, TrackingNumber)
                                                    VALUES (@OrderId, @ShipperId, @ShipmentDate, @TrackingNumber)";

                    if (order.Shipment != null)
                    {
                        int shipmentRowsAffected = connection.Execute(createShipmentSql, new
                        {
                            OrderId = order.Shipment.OrderId,
                            ShipperId = order.Shipment.ShipmentId,
                            ShipmentDate = order.Shipment.ShipmentDate.ToString("yyyy-MM-dd"),
                            TrackingNumber = order.Shipment.TrackingNumber
                        });
                    }

                    //transaction.Commit();
                }
                catch (Exception ex)
                {
                    //transaction.Rollback();
                    throw;
                }
                finally
                {
                    connection.Close();
                }


                

            }

            return order.OrderId;
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

        public ICollection<Order> GetAllOrdersByCustomerId(string customerId)
        {
            return _context.Orders
                .Where(o => o.CustomerId == customerId)
                .OrderByDescending(o => o.OrderDate)
                .ThenByDescending(o => o.OrderId)
                .ToList();
        }
    }
}
