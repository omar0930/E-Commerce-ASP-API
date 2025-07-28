using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Repository.Specifications.ProductSpecs;
using Store.Services.Services;
using Store.Web.Helper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Store.Web.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            this._productService = productService;
        }
        // GET: api/<ProductsController>
        [HttpGet]
        [Cache(30)]
        public async Task<IEnumerable<ProductDetailsDto>> Get([FromQuery] ProductSpecifications input)
        {
            return await this._productService.GetAllProductsWithSpecs(input);
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        public async Task<ProductDetailsDto> Get(int id)
        {
            return await this._productService.GetProductById(id);
        }

        [HttpGet("{id}")]
        public async Task<ProductDetailsDto> GetProductByIdWithSpecs(int id)
        {
            return await this._productService.GetProductByIdWithSpecs(id);
        }

        [HttpGet]
        [Cache(30)]
        public async Task<PaginatedResultDto<ProductDetailsDto>> GetAllProductsWithPaging([FromQuery] ProductSpecifications input)
        {
            return await this._productService.GetAllProductsWithPaging(input);
        }

        // POST api/<ProductsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ProductsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            this._productService.RemoveProduct(id);
        }
    }
}
