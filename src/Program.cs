using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using thegame.Services;

var builder = WebApplication.CreateBuilder();
builder.Services.AddMvc();
builder.Services.AddScoped<IGamesRepository, GamesRepository>();

builder.Services.AddAutoMapper(cfg =>
{
    // cfg.CreateMap<UserEntity, UserDto>()
    //     .ForMember(dest => dest.FullName, opt => opt.MapFrom(src =>
    //         $"{src.LastName} {src.FirstName}"));
    //
    // cfg.CreateMap<UserToCreateDto, UserEntity>();
    // cfg.CreateMap<UserToUpdateDto, UserEntity>();
    // cfg.CreateMap<UserEntity, UserToUpdateDto>();
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