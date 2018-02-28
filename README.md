# CocoHub
An implementation of google's dapper by .NET Core.

## How to use in your code
1. Add reference to the nuget package of Tracer.Fody
```
Install-Package Tracer.Fody
```

2. Add reference to the nuget package of Tracer.Cocohub
```
'Install-Package Tracer.Cocohub
```

3. Add FodyWeavers.xml

4. Add code
```
public void ConfigureServices(IServiceCollection services)
{
  ...
  services.AddCocohub();
  ...
}

public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider svp)
{
  ...
  //remember always to call this brfore UseMvc()
  app.UseCocohub();
  
  app.UseMvc();
  ...
}
```
