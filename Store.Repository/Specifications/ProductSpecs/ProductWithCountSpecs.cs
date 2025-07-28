using Microsoft.IdentityModel.Tokens;
using Store.Data.Entities;
using Store.Repository.Specifications.ProductSpecs;

namespace Store.Repository.Specifications
{
    public class ProductWithCountSpecs : BaseSpecifications<Product>
    {
        public ProductWithCountSpecs(ProductSpecifications specs) : base(product => (!specs.BrandId.HasValue || product.BrandId == specs.BrandId.Value) &&
                (!specs.CategoryId.HasValue || product.CategoryId == specs.CategoryId.Value) && (specs.Search.IsNullOrEmpty() || product.Name.Contains(specs.Search))
             )
        {

        }
    }
}
