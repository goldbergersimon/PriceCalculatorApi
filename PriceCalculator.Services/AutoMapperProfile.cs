using AutoMapper;
using PriceCalculatorApi.Data;
using PriceCalculatorApi.Models;
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


        CreateMap<ProductIngredientEditModel, ProductIngredient>()
            .EqualityComparison((src, dest) => src.ProductIngredientId == dest.ProductIngredientId);

        CreateMap<ProductLaborEditModel, ProductLabor>()
            .EqualityComparison((src, dest) => src.Id == dest.Id);

        CreateMap<ProductIngredient, ProductIngredientEditModel>()
            .ForMember(dest => dest.IngredientId, opt => opt.MapFrom(src => src.IngredientId))
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId));

        CreateMap<ProductLabor, ProductLaborEditModel>()
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId));


        CreateMap<Product, ProductListModel>();
        CreateMap<Product, ProductModel>()
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.ProductIngredients))
            .ForMember(dest => dest.Labors, opt => opt.MapFrom(src => src.ProductLabors));

        CreateMap<ProductModel, Product>()
            .ForMember(dest => dest.ProductIngredients, opt => opt.MapFrom(src => src.Ingredients))
            .ForMember(dest => dest.ProductLabors, opt => opt.MapFrom(src => src.Labors));


        CreateMap<ProductEditModel, Product>()
            .ForMember(dest => dest.ProductIngredients, opt => opt.MapFrom(src => src.Ingredients))
            .ForMember(dest => dest.ProductLabors, opt => opt.MapFrom(src => src.Labors));


        CreateMap<Item, ItemListModel>();

        CreateMap<ItemProductModel, ItemProduct>()
            .EqualityComparison((src, dest)  => src.ItemProductId ==  dest.ItemProductId);

        CreateMap<ItemLaborModel, ItemLabor>()
            .EqualityComparison((src, dest) => src.ItemLaborId == dest.ItemLaborId);

        CreateMap<ItemIngredientModel, ItemIngredient>()
            .EqualityComparison((src, dest) => src.ItemIngredientId == dest.ItemIngredientId);

        CreateMap<ItemProduct, ItemProductModel>()
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.ItemId));

        CreateMap<ItemLabor, ItemLaborModel>()
            .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.ItemId));

        CreateMap<ItemIngredient, ItemIngredientModel>()
            .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.ItemId))
            .ForMember(dest => dest.IngredientId, opt => opt.MapFrom(src => src.IngredientId));

        CreateMap<Item, ItemModel>()
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.ItemProducts))
            .ForMember(dest => dest.Labors, opt => opt.MapFrom(src => src.ItemLabors))
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.ItemIngredients));

        CreateMap<ItemModel, Item>()
            .ForMember(dest => dest.ItemProducts, opt => opt.MapFrom(src => src.Products))
            .ForMember(dest => dest.ItemLabors, opt => opt.MapFrom(src => src.Labors))
            .ForMember(dest => dest.ItemIngredients, opt => opt.MapFrom(src => src.Ingredients));

        CreateMap<ItemEditModel, Item>()
        .ForMember(dest => dest.ItemIngredients, opt => opt.MapFrom(src => src.Ingredients))
        .ForMember(dest => dest.ItemLabors, opt => opt.MapFrom(src => src.Labors))
        .ForMember(dest => dest.ItemProducts, opt => opt.MapFrom(src => src.Products));
    }
}
