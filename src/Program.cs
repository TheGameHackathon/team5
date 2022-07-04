using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using thegame;
using thegame.Models;
using thegame.Services;

var builder = WebApplication.CreateBuilder();
builder.Services.AddMvc();
builder.Services.AddSingleton<IGamesRepository, GamesRepository>();
builder.Services.AddSingleton<IFieldGenerator, FieldGenerator>();

builder.Services.AddAutoMapper(cfg =>
{
    // cfg.CreateMap<UserEntity, UserDto>()
    //     .ForMember(dest => dest.FullName, opt => opt.MapFrom(src =>
    //         $"{src.LastName} {src.FirstName}"));
    //
    // cfg.CreateMap<UserToCreateDto, UserEntity>();
    // cfg.CreateMap<UserToUpdateDto, UserEntity>();
    // cfg.CreateMap<UserEntity, UserToUpdateDto>();
    cfg.CreateMap<FloodFillGame, GameDto>().ForMember(dest => dest.Cells, opt => opt.MapFrom(src => src.Field))
        .ForMember(dest => dest.MonitorMouseClicks, opt => opt.MapFrom(src => true));
}, Array.Empty<Assembly>());

var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseStaticFiles();
app.UseRouting();
app.UseEndpoints(endpoints => endpoints.MapControllers());
app.Use((context, next) =>
{
    context.Request.Path = "/index.html";
    return next();
});
app.UseStaticFiles();


app.Run();