using aspnetserver.Data;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

//CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("COSRPolicy", builder =>
    {
        builder
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithOrigins("http://localhost:3000", "https://appname.azurestaticapps.net");
    });
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swaggerGenOptions =>
{
    swaggerGenOptions.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ASP NET React Tut",
        Version = "v1"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI(swaggerUIOptions =>
{
    swaggerUIOptions.DocumentTitle = "ASP NET React Tut";
    swaggerUIOptions.SwaggerEndpoint("/swagger/v1/swagger.json", "Web API simple model");
    swaggerUIOptions.RoutePrefix = String.Empty;
});


app.UseHttpsRedirection();

app.UseCors("COSRPolicy");

//API Method
app.MapGet("/get-all-posts", async () => await PostsRepository.GetPostsAsync()).WithTags("Posts Endpoints");

app.MapGet("/get-post-by-id/{postId}", async (int postId) => {
    Post postToReturn = await PostsRepository.GetPostByIdAsync(postId);
    if (postToReturn != null)
    {
        return Results.Ok(postToReturn);
    }
    else
    {
        return Results.BadRequest();
    }
}).WithTags("Posts Endpoints");

app.MapPost("/create-post", async (Post postToCreate) => {
    bool isCreated = await PostsRepository.CreatePostAsync(postToCreate);
    if (isCreated)
    {
        return Results.Ok("Berhasil ditembahkan");
    }
    else
    {
        return Results.BadRequest("Gagal ditambahkan");
    }
}).WithTags("Posts Endpoints");


app.MapPut("/update-post", async (Post postToUpdate) => {
    bool isUpdated = await PostsRepository.UpdatePostAsync(postToUpdate);
    if (isUpdated)
    {
        return Results.Ok("Berhasil dirubah");
    }
    else
    {
        return Results.BadRequest("Gagal merubah");
    }
}).WithTags("Posts Endpoints");

app.MapDelete("/delete-post-by-id/{postId}", async (int postId) => {
    bool isDeleted = await PostsRepository.DeletePostAsync(postId);
    if (isDeleted)
    {
        return Results.Ok("Berhasil dihapus");
    }
    else
    {
        return Results.BadRequest("Gagal menghapus");
    }
}).WithTags("Posts Endpoints");



app.Run();

