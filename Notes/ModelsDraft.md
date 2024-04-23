User
    ID
    Username

    Collection
    GameLists
    GameStatuses

GameList
    ID
    Name
    Unordered

    User
    GameListEntries

GameListEntry
    ID
    Number

    Game
    GameList

Collection
    ID

    User
    CollectionGames

CollectionGame
    ID
    TimeAcquired
    Digital: bool

    Collection
    Game
    Platform
    Region

GameStatus
    ID
    Completed
    PlayState

    Game
    User

Game
    ID
    checksum
    created_at
    first_release_date
    name
    parent_game
    version_parent

Platform
    ID
    checksum
    created_at
    first_release_date
    name
    parent_game
    version_parent