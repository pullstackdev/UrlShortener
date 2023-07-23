# UrlShortener
A Simple URL Shortening Service
## Setup Project

1. SQLite and related EF Core libraries are used. Install Nuget Packages.
2. Make migration and db update in Entities directory.
3. Run the project from UrlShortener directory.

## API Doc

### Shorten URL

<details>
 <summary><code>POST</code> <code><b>/shorten</b></code> <code>(Shortens the given url)</code></summary>

##### Parameters

> | name              |  type     | data type      | description                         |
> |-------------------|-----------|----------------|-------------------------------------|
> | `url`             |  required | string         | URL to be shortened                 |
> | `customToken`     |  optional | string         | user selected custom token          |



##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `200`         | `application/json`                | `{"shortenedUrl": "http://www.samplesite.com/123zxc"}`              |
> | `400`         | `application/json`                | `{"error":"Invalid URL"}`                                           |

##### Example cURL

> ```javascript
>    curl --location --request POST 'http://samplesite.com/shorten' \
>    --header 'Content-Type: application/json' \
>    --data-raw '{
>        "url" : "https://github.com/torvalds/linux/pull/437",
>        "customToken" : ""
>    }'
> ```

</details>

------------------------------------------------------------------------------------------

### Redirection

<details>
 <summary><code>GET</code> <code><b>/{token}</b></code> <code>(Redirects to original URL)</code></summary>

##### Parameters

> None

##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `302`         | `text/plain;charset=UTF-8`        | Redirection                                                         |

##### Example cURL

> ```javascript
>  curl --location --request GET 'http://samplesite.com/fKTYsA'
> ```

</details>
