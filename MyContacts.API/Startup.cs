using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyContacts.SqlClient;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AutoMapper;
using MyContacts.API.Models;
using MyContacts.SqlClient.Tables;
using MyContacts.Core.Services.Contacts;

namespace MyContacts.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

           // Add database config 
            services.AddDbContext<MyContactsContext>(opt => {
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            // Add Cors 
            services.AddCors(opt => {
                opt.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyHeader()
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .Build()
                );
            });

            // Configure OAuth token based authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt => {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true, // Validate the server that created that token 
                        ValidateAudience = true, // Ensure the reciepient of the token is authrized to receive it
                        ValidateLifetime = true, // Check that the token is not expired and that the signing key of the issuer is valid
                        ValidateIssuerSigningKey = true, // Verify that the key used to sign the incoming token is part of a list of trusted keys
                        ValidIssuer = Configuration["JWT:Issuer"],
                        ValidAudience = Configuration["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Key"]))
                    };
                });
            // Configure mapper
            services.AddAutoMapper();

            services.AddScoped<IContactsRepository, ContactsRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");

            // Use the JWT authentication CONFIGURATION
            app.UseAuthentication();


            // Instantiate auto mapper
            AutoMapper.Mapper.Initialize(config =>
            {
                config.CreateMap<ContactViewModel, ContactDto>().ReverseMap();
                config.CreateMap<IEnumerable<ContactViewModel>, IEnumerable<ContactDto>>().ReverseMap();
            });

            app.UseMvc();
        }
    }
}
