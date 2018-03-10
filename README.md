# CocoHub
An implementation of google's dapper by .NET Core.

## How to use in your code
1. Add reference to the nuget package of Fody
```
Install-Package Fody
```

2. Add reference to the nuget package of Cocohub.Tracer.Fody
```
Install-Package Cocohub.Tracer.Fody
```

3. Add reference to the nuget package of Tracer.Cocohub
```
'Install-Package Tracer.Cocohub
```

4. Add FodyWeavers.xml

5. Add code
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
