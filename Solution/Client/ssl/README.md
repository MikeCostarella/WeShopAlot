# Dev HTTPS certificate

`npm start` runs `prestart` first, which exports the ASP.NET Core development certificate here as
`localhost.pem` and `localhost.key`. `ng serve` uses them for https://localhost:4200, so the Angular
dev server and the API (https://localhost:7244) share one certificate.

If Chrome warns that the site is not secure, trust the certificate once (PowerShell):

```
dotnet dev-certs https --trust
```

The files are per machine and are not committed.
