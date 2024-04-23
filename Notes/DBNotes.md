User
    ID:
    Username:

GameList
    ID:
    UserID:
    Name: string
    Unordered: bool

GameListEntry
    ID
    GameID // The parent most version
    ListID
    Number

GameStatus
    ID
    UserID
    GameID
    Completed: bool
    PlayState
    Time Started
    Time Finished
    Time Added

Collection
    ID:
    UserID:

CollectionGame
    ID
    CollectionID
    GameID
    Time Acquired
    Platform
    Physical/Digital
    Region


ExternalGameInfo
    HLTB
    PriceCharting
    OpenCritic

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
    abbreviation
    alt_name
    checksum
    created_at
    generation
    name
    updated_at

--------------------
Extra Platform Stuff
--------------------
Platform Family
https://api.igdb.com/v4/platform_families
    id
    checksum
    name
    slug

Platform Logo

Platform Category (enum)
    console
    arcade
    platform
    os
    portable
    computer

Region
category 	String 	This can be either ’locale’ or ‘continent’
checksum 	uuid 	Hash of the object
created_at 	Unix Time Stamp 	Date this was initially added to the IGDB database
identifier 	String 	This is the identifier of each region
name 	String 	
updated_at 	Unix Time Stamp 	The last date this entry was updated in the IGDB database