using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Store.Data.Entities;
using Store.Repository.Interfaces;
using Store.Repository.Specifications;
using Store.Repository.Specifications.ProductSpecs;

namespace Store.Services.Services
{
    public class ProductService : IProductService
    {
        private IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        public ProductService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            this._unitOfWork = unitOfWork;
            this._configuration = configuration;
        }
        public async Task<IEnumerable<ProductDetailsDto>> GetAllProducts()
        {
            var products = await _unitOfWork.Repository<Product, int>().GetAll().Include(p => p.Category)
                                     .Include(p => p.Brand)
                                     .ToListAsync();

            // map products to ProductDetailsDto
            // concat baseUrl located in Appsetting.json using IConfiguring with image url
            var baseUrl = _configuration["BaseUrl"];
            var mappedProducts = products.Select(p => new ProductDetailsDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                CategoryName = p.Category.Name,
                PictureUrl = baseUrl + p.PictureUrl,
                BrandName = p.Brand.Name,
            });

            return mappedProducts;
        }

        public async Task<IEnumerable<ProductDetailsDto>> GetAllProductsWithSpecs(ProductSpecifications specs)
        {
            var ProductSpec = new ProductWithSpecifications(specs);

            var products = await _unitOfWork.Repository<Product, int>().GetAllWithSpecifications(ProductSpec);

            var mappedProducts = products.Select(product => new ProductDetailsDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryName = product.Category?.Name, // Handle nulls if needed
                PictureUrl = product.PictureUrl,
                BrandName = product.Brand?.Name // Handle nulls if needed
            }).ToList();

            return mappedProducts;
        }

        public async Task<PaginatedResultDto<ProductDetailsDto>> GetAllProductsWithPaging(ProductSpecifications specs)
        {
            var ProductSpec = new ProductWithSpecifications(specs);

            var products = await _unitOfWork.Repository<Product, int>().GetAllWithSpecifications(ProductSpec);

            var mappedProducts = products.Select(product => new ProductDetailsDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryName = product.Category?.Name, // Handle nulls if needed
                PictureUrl = product.PictureUrl,
                BrandName = product.Brand?.Name // Handle nulls if needed
            }).ToList();

            var ProductsCountSpec = new ProductWithCountSpecs(specs);

            var productsCount = await _unitOfWork.Repository<Product, int>().GetProductsCount(ProductsCountSpec);

            return new PaginatedResultDto<ProductDetailsDto>(productsCount, specs.PageIndex, specs.PageSize, mappedProducts);
        }

        public async Task<ProductDetailsDto> GetProductByIdWithSpecs(int id)
        {
            var specs = new ProductWithSpecifications(id);

            var product = await _unitOfWork.Repository<Product, int>().GetByIdWithSpecifications(specs);

            var mappedProduct = new ProductDetailsDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryName = product.Category?.Name, // Handle nulls if needed
                PictureUrl = product.PictureUrl,
                BrandName = product.Brand?.Name // Handle nulls if needed
            };

            return mappedProduct;
        }
        public async Task<ProductDetailsDto> GetProductById(int id)
        {
            var product = await _unitOfWork.Repository<Product, int>().GetAll().Include(p => p.Category)
                                     .Include(p => p.Brand).FirstOrDefaultAsync(p => p.Id == id);

            // concat baseUrl located in Appsetting.json using IConfiguring with image url
            var baseUrl = _configuration["BaseUrl"];
            var mappedProduct = new ProductDetailsDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryName = product.Category.Name,
                PictureUrl = baseUrl + product.PictureUrl,
                BrandName = product.Brand.Name,
                Category = product.Category,
                Brand = product.Brand
            };
            return mappedProduct;
        }
        public void RemoveProduct(int id)
        {
            var productEntity = _unitOfWork.Repository<Product, int>().GetAll().FirstOrDefault(p => p.Id == id);

            this._unitOfWork.Repository<Product, int>().DeleteAsync(productEntity);
        }

    }
}
