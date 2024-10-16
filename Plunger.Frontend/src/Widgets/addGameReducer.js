function addGameReducer(formState, action) {
    switch(action.type) {
        case 'searchTextChanged': {
            let text = action.payload;
            return {
                ...formState,
                searchField: text
            };
        }
        case 'gameSelected': {
            let game = action.payload;
            return {
                ...formState,
                game: game,
                searchField: game.name,
                region: game.regions[0].id,
                platformId: game.platforms[0].id
            };
        }
        case 'regionSelected': {
            let region = action.payload;
            return {
                ...formState,
                region: region
            };
        }
        case 'platformSelected': {
            let platformId = action.payload;
            return {
                ...formState,
                platformId: platformId
            }
        }
        case 'tangibilitySelected': {
            let tangibility = action.payload;
            return {
                ...formState,
                tangibility: tangibility
            };
        }
        case 'priceChanged': {
            let price = action.payload;
            return {
                ...formState,
                price: price,
            };
        }
        case 'dateSelected': {
            let date = action.payload;
            return {
                ...formState,
                dateAcquired: date
            };
        }
    }
}

export default addGameReducer;