# deeplinks.help

## Debugging locally

When running the frontend from `file://`, the CORS origin is null. To make the backend allow it, create an `appsettings.json.local` with the following content:
```json
{
  "AllowedCorsOrigin": "null"
}
```
