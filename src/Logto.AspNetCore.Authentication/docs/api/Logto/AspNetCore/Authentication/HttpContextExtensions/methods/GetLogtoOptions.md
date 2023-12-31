﻿<!--  
  <auto-generated>   
    The contents of this file were generated by a tool.  
    Changes to this file may be list if the file is regenerated  
  </auto-generated>   
-->

# HttpContextExtensions.GetLogtoOptions Method

**Declaring Type:** [HttpContextExtensions](../index.md)  
**Namespace:** [Logto.AspNetCore.Authentication](../../index.md)  
**Assembly:** Logto.AspNetCore.Authentication  
**Assembly Version:** 0.1.0\-alpha.0

## Overloads

| Signature                                                                  | Description |
| -------------------------------------------------------------------------- | ----------- |
| [GetLogtoOptions(HttpContext)](#getlogtooptionshttpcontext)                |             |
| [GetLogtoOptions(HttpContext, string)](#getlogtooptionshttpcontext-string) |             |

## GetLogtoOptions(HttpContext)

```csharp
public static LogtoOptions GetLogtoOptions(this HttpContext httpContext);
```

### Parameters

`httpContext`  HttpContext

### Returns

[LogtoOptions](../../LogtoOptions/index.md)

## GetLogtoOptions(HttpContext, string)

```csharp
public static LogtoOptions GetLogtoOptions(this HttpContext httpContext, string authenticationScheme);
```

### Parameters

`httpContext`  HttpContext

`authenticationScheme`  string

### Returns

[LogtoOptions](../../LogtoOptions/index.md)

___

*Documentation generated by [MdDocs](https://github.com/ap0llo/mddocs)*
