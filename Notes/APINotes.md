User-facing
    PlayStatus
        Edit GameStatus
    Collection
        Add Game
        Remove 
    Queue
        GetQueue
        GetNowPlaying
        GetRecent
    List
        Create List
        Read List
        Modify List
        Delete List

Backend-facing
    Game Library related APIs
        Add Game
        Add Version

    Add Game

***FIGURE OUT API RESPONSES***

POST:   /api/user/create - Create User
POST:   /api/user/login - Login
POST:   /api/user/logout - Logout

POST:   /api/games - Create Game
GET:    /api/games/{gameid} - Get Game
PATCH:  /api/games/{gameid} - Update Game
DELETE: /api/games/{gameid} - Delete Game

POST:   /api/list/ - Create List
GET:    /api/list/{listid} - Get List
PATCH:  /api/list/{listid} - Update List
DELETE: /api/list/{listid} - Delete List

GET:    /api/collection/{collectionid} - Get Collection
PATCH:  /api/collection/{collectionid} - Update Collection
    * Add Game
    * Remove Game

GET:    /api/users/{userid}/collections
GET:    /api/users/{userid}/lists

POST:   /api/users/{userid}/games
GET:    /api/users/{userid}/games/{gameid} - Get GameStatus
PATCH:  /api/users/{userid}/games/{gameid} - Update GameStatus