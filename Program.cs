using AIChat.Interfaces;
using AIChat.Repositories;
using AIChat.Services;
using AIChat.Configuration;
using AIChat.Middleware;
using AIChat.Factories;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using AIChat.Data;
using Microsoft.EntityFrameworkCore;

namespace AIChat
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.Configure<AIOptions>(builder.Configuration.GetSection("AI"));
            //builder.Services.AddHttpClient();
            builder.Services.AddHttpClient<IOpenRouterClient,OpenRouterClient>((serviceProvider, client) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<AIOptions>>().Value;
                client.BaseAddress = new Uri(options.BaseUrl);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", options.ApiKey);
            });
            builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));  
            builder.Services.AddScoped<IChatService, ChatService>();
            builder.Services.AddScoped<IOpenRouterRequestFactory, OpenRouterRequestFactory>();
            builder.Services.AddScoped<IConversationRepository, ConversationRepository>();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("ReactFrontend", policy =>
                {
                    policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod();
                });
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseHttpsRedirection();

            app.UseCors("ReactFrontend");

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
