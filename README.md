# sdk-dotnet

This is the base client for interacting with UKFast APIs from .NET applications targetting .NET Standard 2.0.

We strongly recommend against using this client directly, and instead using UKFast .NET SDKs available on Github [here](https://github.com/ukfast?utf8=%E2%9C%93&q=sdk-dotnet)

### Basic usage

Whilst it's recommended to use the available SDKs as mentioned above, direct usage of the client is possible.
You should refer to the [Getting Started](https://developers.ukfast.io/getting-started) section of the API documentation before proceeding below

First, we'll instantiate an instance of `UKFastClient`:

```csharp
using UKFast.API.Client;

UKFastClient client = new UKFastClient(new ClientConnection("yourapikeyhere"));
```

And away we go:

```csharp
var zone = await client.GetAsync<dynamic>("/safedns/v1/zones/ukfast.co.uk");
```
