using AutoMapper;
using PriceCalculatorApi.Data;
using PriceCalculatorApi.Models;

namespace PriceCalculatorApi.Services;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Product, ProductListModel>();
        CreateMap<Product, ProductModel>();
        CreateMap<Ingredient, IngredientModel>();
        CreateMap<IngredientEditModel, Ingredient>();
        CreateMap<Ingredient, IngredientEditModel>();
        CreateMap<Item, ItemListModel>();
        CreateMap<IngredientModel, Ingredient>();
    }
}
