using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VivesRental.Model;
using VivesRental.Repository.Core;
using VivesRental.Enums;

namespace VivesRental.Api.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<VivesRentalDbContext>();

            // If products already exist assume DB was seeded
            if (await context.Products.AnyAsync())
                return;

            // Customers
            var cust1 = new Customer
            {
                Id = Guid.NewGuid(),
                FirstName = "Student",
                LastName = "One",
                Email = "student.one@local.com",
                PhoneNumber = "0470000001"
            };

            var cust2 = new Customer
            {
                Id = Guid.NewGuid(),
                FirstName = "Student",
                LastName = "Two",
                Email = "student.two@local.com",
                PhoneNumber = "0470000002"
            };

            var cust3 = new Customer
            {
                Id = Guid.NewGuid(),
                FirstName = "Lab",
                LastName = "Assistant",
                Email = "lab.assistant@local.com",
                PhoneNumber = "0470000003"
            };

            context.Customers.AddRange(cust1, cust2, cust3);

            // Student-useful products
            var laptop = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Laptop (Dell XPS 13)",
                Description = "Portable laptop suitable for development and coursework",
                Manufacturer = "Dell",
                RentalExpiresAfterDays = 14
            };

            var textbook = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Textbook: Programming in C#",
                Description = "Reference textbook for C# programming",
                Manufacturer = "TechBooks",
                RentalExpiresAfterDays = 28
            };

            var multimeter = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Digital Multimeter",
                Description = "Handheld multimeter for electrical measurements",
                Manufacturer = "TestTools",
                RentalExpiresAfterDays = 7
            };

            var oscilloscope = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Oscilloscope (100MHz)",
                Description = "Benchtop oscilloscope for signal analysis",
                Manufacturer = "ScopeCorp",
                RentalExpiresAfterDays = 7
            };

            var projector = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Portable Projector",
                Description = "Small projector for presentations",
                Manufacturer = "ShowTech",
                RentalExpiresAfterDays = 7
            };

            var camera = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Digital Camera",
                Description = "Compact camera for media projects",
                Manufacturer = "CamVision",
                RentalExpiresAfterDays = 7
            };

            var headset = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Headset with Microphone",
                Description = "Wired headset for online meetings and recordings",
                Manufacturer = "SoundPro",
                RentalExpiresAfterDays = 14
            };

            context.Products.AddRange(laptop, textbook, multimeter, oscilloscope, projector, camera, headset);

            // Create articles (stock items) for each product
            var laptopArticles = Enumerable.Range(1, 6).Select(_ => new Article
            {
                Id = Guid.NewGuid(),
                ProductId = laptop.Id,
                Status = ArticleStatus.Normal
            }).ToList();

            var textbookArticles = Enumerable.Range(1, 10).Select(_ => new Article
            {
                Id = Guid.NewGuid(),
                ProductId = textbook.Id,
                Status = ArticleStatus.Normal
            }).ToList();

            var multimeterArticles = Enumerable.Range(1, 5).Select(_ => new Article
            {
                Id = Guid.NewGuid(),
                ProductId = multimeter.Id,
                Status = ArticleStatus.Normal
            }).ToList();

            var scopeArticles = Enumerable.Range(1, 2).Select(_ => new Article
            {
                Id = Guid.NewGuid(),
                ProductId = oscilloscope.Id,
                Status = ArticleStatus.Normal
            }).ToList();

            var projectorArticles = Enumerable.Range(1, 2).Select(_ => new Article
            {
                Id = Guid.NewGuid(),
                ProductId = projector.Id,
                Status = ArticleStatus.Normal
            }).ToList();

            var cameraArticles = Enumerable.Range(1, 3).Select(_ => new Article
            {
                Id = Guid.NewGuid(),
                ProductId = camera.Id,
                Status = ArticleStatus.Normal
            }).ToList();

            var headsetArticles = Enumerable.Range(1, 8).Select(_ => new Article
            {
                Id = Guid.NewGuid(),
                ProductId = headset.Id,
                Status = ArticleStatus.Normal
            }).ToList();

            context.Articles.AddRange(
                laptopArticles
                .Concat(textbookArticles)
                .Concat(multimeterArticles)
                .Concat(scopeArticles)
                .Concat(projectorArticles)
                .Concat(cameraArticles)
                .Concat(headsetArticles)
            );

            await context.SaveChangesAsync();

            // Create a sample order for cust1 using one laptop and one textbook
            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = cust1.Id,
                CustomerFirstName = cust1.FirstName,
                CustomerLastName = cust1.LastName,
                CustomerEmail = cust1.Email,
                CustomerPhoneNumber = cust1.PhoneNumber,
                CreatedAt = DateTime.UtcNow
            };

            context.Orders.Add(order);
            await context.SaveChangesAsync();

            var rentedAt = DateTime.UtcNow;

            var firstLaptop = laptopArticles.FirstOrDefault();
            var firstTextbook = textbookArticles.FirstOrDefault();

            if (firstLaptop != null)
            {
                context.OrderLines.Add(new OrderLine
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,
                    ArticleId = firstLaptop.Id,
                    ProductName = laptop.Name,
                    ProductDescription = laptop.Description,
                    RentedAt = rentedAt,
                    ExpiresAt = rentedAt.AddDays(laptop.RentalExpiresAfterDays)
                });
            }

            if (firstTextbook != null)
            {
                context.OrderLines.Add(new OrderLine
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,
                    ArticleId = firstTextbook.Id,
                    ProductName = textbook.Name,
                    ProductDescription = textbook.Description,
                    RentedAt = rentedAt,
                    ExpiresAt = rentedAt.AddDays(textbook.RentalExpiresAfterDays)
                });
            }

            // Reservation: reserve one multimeter for cust2 tomorrow
            var reservedMultimeter = multimeterArticles.FirstOrDefault();
            if (reservedMultimeter != null)
            {
                var reservation = new ArticleReservation
                {
                    Id = Guid.NewGuid(),
                    ArticleId = reservedMultimeter.Id,
                    CustomerId = cust2.Id,
                    FromDateTime = DateTime.UtcNow.AddDays(1),
                    UntilDateTime = DateTime.UtcNow.AddDays(3)
                };
                context.ArticleReservations.Add(reservation);
            }

            await context.SaveChangesAsync();
        }
    }
}
