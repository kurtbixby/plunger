# 2/18/24

## To Do

- Block out header bar
- Design/style homepage elements
- Implement more functionality
  - Emulate sending and receiving from a backend
  - Flesh out the mock data

# 6/18/24

- /api/GetCurrentUser
  - Populate UserContext specifically the token
  - Hit endpoint every refresh
    - Refresh 
    - If rejected, clear cookies, delete user object
    - 
- /


need refresh, navigation, etc
Misc thoughts on homepage

AddGameWidget contains AddGameForm

customizable
  {
    id
    userId
    kind
    size
    sourceList
  }
  Procedural (generated)
    Kind
      Now Playing
      Recently Started
      Recently Acquired
  Static
    List ID
    Number of entries

Profile, Collection, Games, Lists

home/{username}
home/{username}/collection
home/{username}/lists

CreateUser widget
Persistent login
List page

Implement Content Security Policy
https://cheatsheetseries.owasp.org/cheatsheets/Content_Security_Policy_Cheat_Sheet.html

Add endpoint to login with access token

Add device id to main token

Test UserDetails in TokenLogin Endpoint
Test frontend for login

Create two tokens
  Access token stored in cookie (session?)
  Refresh token store in JS closure
    Refresh sent when < 24 hours left until access token expiration
  Endpoint to regenerate refresh token
  Startup logic sends access token to login
    Updates access token

Cover notes
Use baseurl + imageid and fill in which variant dynamically depending on situation
Need default image for coverless games