# sdk-dotnet

[![MIT licensed](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

This is the base client for interacting with ANS APIs from .NET applications targetting .NET Standard 2.0.

We strongly recommend against using this client directly, and instead using ANS .NET SDKs available on Github [here](https://github.com/ans-group?utf8=%E2%9C%93&q=sdk-dotnet)

### Basic usage

Whilst it's recommended to use the available SDKs as mentioned above, direct usage of the client is possible.
You should refer to the [Getting Started](https://developers.ukfast.io/getting-started) section of the API documentation before proceeding below

First, we'll instantiate an instance of `ANSClient`:

```csharp
using ANS.API.Client;

ANSClient client = new ANSClient(new ClientConnection("yourapikeyhere"));
```

And away we go:

```csharp
var zone = await client.GetAsync<dynamic>("/safedns/v1/zones/ANS.co.uk");
```
