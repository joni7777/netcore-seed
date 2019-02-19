# bp-netcore-seed
Bp dotnet core seed for microservices

# How to use
Fork the repo to A new git repo, using the naming convention: `bp-{SERVICE_NAME}-service`
Rename the sln file in the new repo, to the service name, `Bp{ServiceName}Service.sln`
Rename in the `Config\appsettings.json` the service info (version and name)

# How to add components
Under the component project, will be all of the microservice code
Each component will be in A new folder, like Sample and Sample can be removed
If some common dto's/functions/models are needed, it could also be added to Bp.Common package

# Common configuration info
Config is loaded into the project using netcore environment
It assumes the config will be in directory in the main seed project
and it will loads the configs by the order:

1. `appsettings.json`
2. `appsettings.{Environment}.json`
3. `appsettings.{Environment}.local.json`

and then the environment variables

## Bp.Logging
Add default logger sinks for mattermost and splunk and console

### Splunk
For splunk logger have in the config:
```json
{
    "Serilog": {
        ...
        "CustomLoggers": {
            ...
            "SplunkLogger": {
                "Protocol": "http",
                "Host": "localhost",
                "Port": "3000",
                "Username": "Username",
                "Password": "Password",
                "SourceType": "SourceType",
                "Index": "Index"
            },
            ...
        },
        ...
    }
}
```

### Mattermost
For mattermost logger have in the config:
```json
{
    "Serilog": {
        ...
        "CustomLoggers": {
            ...
            "MattermostLogger": {
                "Protocol": "http",
                "Host": "localhost",
                "Port": "3000",
                "Path": "bla/6"
            },
            ...
        },
        ...
    }
}
```

Mattermost by default will get only logs from Error level
Splunk by default will get only logs from Debug level

## Change default port
### Development
In appsettings.json change the config:
```json
{
    ...
    "Kestrel": {
        "EndPoints": {
            "Http": {
                "Url": "http://*:{PORT}"
            }
        }
    },
    ...
}
```
### Production
```json
{
    ...
    "Kestrel": {
        "EndPoints": {
            "Https": {
                "Url": "https://*:{PORT}"
            }
        }
    },
    ...
}
```

## Default health checks
There is some default health checks for different dbs

### Sql Server
```json
{
    ...
    "Data": {
        "SqlServer": {
            "ConnectionString": "{SQL_CONNECTION_STRING}"
        }
    },
    ...
}
```
### Mongo Server
```json
{
    ...
    "Data": {
        "MongoDB": {
            "ConnectionString": "{SQL_CONNECTION_STRING}"
        }
    },
    ...
}
```

# Extend Bp.ApiRunner configure services
Allow extending configure services of the ApiRunner
Using reflection:
1. loads the EntryAssembly
2. looks for Class with the name: `BpConfigureServices`
3. look for A static public method: `ExtendConfigureServices` AND OR `ExtendConfigure` 
4. invoke the method(s) from before with the default startup arguments (`IServiceCollection`, `IConfiguration` for `ExtendConfigureServices` and `IApplicationBuilder`, `IHostingEnvironment`, `IConfiguration` for `ExtendConfigure`) 
If the class or one of the methods are null it just skip the null class or method