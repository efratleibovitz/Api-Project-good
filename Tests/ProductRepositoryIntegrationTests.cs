using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Repository;

namespace Tests
{
   

    public class ProductRepositoryIntegrationTests : IClassFixture<DbFixture>
    {
        private readonly WebApiShop216328971Context _context;
        private readonly ProductRepository _repository;

        public ProductRepositoryIntegrationTests(DbFixture fixture)
        {
            _context = fixture.Context;
            _repository = new ProductRepository(_context);
            _context.ChangeTracker.Clear();
        }
        #region happy tests
        [Fact]
        public async Task GetProducts_ShouldReturnAllProducts()
        {
            // Arrange
            var product = new Product { ProductName = "TestProduct", Price = 50 };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetProducts(null, null, 0, 0, null,null,null,null);

            // Assert
            Assert.NotNull(result);
            Assert.Contains(result.Items, p => p.ProductName == "TestProduct");
        }
        #endregion

        #region unhappy tests

        [Fact]
        public async Task GetProducts_WhenNoProducts_ReturnsEmptyList()
        {
            var result = await _repository.GetProducts(null, null, 0, 0, null, null, null, null);

            Assert.NotNull(result);
            Assert.Empty(result.Items);
        }

        #endregion
    }
}
