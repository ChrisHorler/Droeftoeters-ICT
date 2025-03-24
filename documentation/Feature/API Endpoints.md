---
tags:
  - author/martijn
  - back-end
  - feature/login
---

# Login Related Endpoints

## Check Access Token
- Method: `POST`
- Authorization: `true`
- Initiation location: `Program.cs`


On the `/account/checkAccessToken` endpoint you can now check whether you are logged in or not, or more accurately: whether your sessiontoken is valid.

If the token is valid you'll get an `200` response:

```json
{
	"authorized": true
}
```

When your sessiontoken is invalid, it'll give a `401`, just like any other request with the `[Authorize]`.