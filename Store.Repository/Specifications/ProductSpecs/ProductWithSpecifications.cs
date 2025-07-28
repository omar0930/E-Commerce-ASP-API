using Microsoft.IdentityModel.Tokens;
using Store.Data.Entities;
using Store.Repository.Specifications.ProductSpecs;

namespace Store.Repository.Specifications
{
    public class ProductWithSpecifications : BaseSpecifications<Product>
    {
        public ProductWithSpecifications(ProductSpecifications specs) :
             base(product => (!specs.BrandId.HasValue || product.BrandId == specs.BrandId.Value) &&
                (!specs.CategoryId.HasValue || product.CategoryId == specs.CategoryId.Value) && (specs.Search.IsNullOrEmpty() || product.Name.Contains(specs.Search))
             )
        {
            AddInclude(product => product.Brand);
            AddInclude(product => product.Category);
            ApplyPaging(specs.PageSize * (specs.PageIndex - 1), specs.PageSize);

            if (!String.IsNullOrEmpty(specs.Sort))
            {
                switch (specs.Sort)
                {
                    case "priceAsc":
                        AddOrderByAsc(product => product.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDesc(product => product.Price);
                        break;

                    case "brandAsc":
                        AddOrderByAsc(product => product.Brand.Name);
                        break;

                    case "brandDesc":
                        AddOrderByDesc(product => product.Brand.Name);
                        break;

                    default:
                        AddOrderByAsc(product => product.Name);
                        break;
                }
            }
        }
        public ProductWithSpecifications(int? id) :
             base(product => product.Id == id)
        {
            AddInclude(product => product.Brand);
            AddInclude(product => product.Category);
        }
    }
}
