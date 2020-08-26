// - Required Assemblies
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// - Application Assemblies
using MyPeeps.Ui.Repositories;

namespace MyPeeps
{
  public class Startup
  {
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMvc();

      // - Switch the Repository here...
      //   to work from Mock Repository or Service Repository
      services.AddSingleton<IPhoneBookRepository, PhoneBookRepositoryMock>();
      //services.AddSingleton<IPhoneBookRepository, PhoneBookRepositoryToService>();

      services.AddHttpClient();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder applicationBuilder, 
                          IWebHostEnvironment webHostEnvironment)
    {
      if (webHostEnvironment.IsDevelopment())
      {
        applicationBuilder.UseDeveloperExceptionPage();
      }

      // - Serve Static Files
      applicationBuilder.UseDefaultFiles();
      applicationBuilder.UseStaticFiles();

      // - MVC
      applicationBuilder.UseRouting();
      applicationBuilder.UseEndpoints(endpoints => { endpoints.MapControllerRoute("default", "{controller=PhoneBook}/{action=GetPhoneBooks}"); });

    }
  }
}
