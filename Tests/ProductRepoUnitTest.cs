using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Repository;

namespace Tests
{
    public class ProductRepoUnitTest : TestBase
    {
        #region happy tests
        [Fact]
        public async Task GetProducts_ReturnsAllProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { ProductId = 1, ProductName = "Cake" },
                new Product { ProductId = 2, ProductName = "Cookie" }
            };

            var mockContext =
                GetMockContext<WebApiShop216328971Context, Product>(products, c => c.Products);

            var repo = new ProductRepository(mockContext.Object);

            // Act
            var result = await repo.GetProducts(null, null, 0, 0, null, null, null, null);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.TotalCount);
            Assert.Equal("Cake", result.Items[0].ProductName);
        }

        #endregion

        #region unhappy tests
        //אין מוצרים במערכת
        [Fact]
        public async Task GetProducts_NoProducts_ReturnsEmptyList()
        {
            // Arrange
            var mockContext =
                GetMockContext<WebApiShop216328971Context, Product>(
                    new List<Product>(), c => c.Products);

            var repo = new ProductRepository(mockContext.Object);

            // Act
            var result = await repo.GetProducts(null, null, 0, 0, null, null, null, null);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Items);
        }
        #endregion
    }

}

