using AutoMapper;
using PriceCalculatorApi.Data;
using PriceCalculatorApi.Models;
using AutoMapper.Collection;
using AutoMapper.EquivalencyExpression;

namespace PriceCalculatorApi.Services;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {

        CreateMap<Ingredient, IngredientModel>();
        CreateMap<IngredientEditModel, Ingredient>();
        CreateMap<Ingredient, IngredientEditModel>();
        CreateMap<IngredientModel, Ingredient>();

        CreateMap<Item, ItemListModel>();


        CreateMap<ProductIngredientEditModel, ProductIngredient>()
            .EqualityComparison((src, dest) => src.ProductIngredientID == dest.ProductIngredientID);

        CreateMap<ProductIngredient, ProductIngredientEditModel>()
            .ForMember(dest => dest.IngredientID, opt => opt.MapFrom(src => src.IngredientID))
            .ForMember(dest => dest.ProductID, opt => opt.MapFrom(src => src.ProductID));

        CreateMap<ProductLabor, ProductLaborEditModel>()
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId));


        CreateMap<ProductLaborEditModel, ProductLabor>()
            .EqualityComparison((src, dest) => src.Id == dest.Id);


        CreateMap<Product, ProductListModel>();
        CreateMap<Product, ProductModel>()
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.ProductIngredients))
            .ForMember(dest => dest.Labors, opt => opt.MapFrom(src => src.ProductLabors));


        CreateMap<ProductEditModel, Product>()
            .ForMember(dest => dest.ProductIngredients, opt => opt.MapFrom(src => src.Ingredients))
            .ForMember(dest => dest.ProductLabors, opt => opt.MapFrom(src => src.Labors));

    }
}
