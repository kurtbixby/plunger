import {createContext, useContext, useReducer, useState} from "react";

const UserContext = createContext(null);

function currentUserReducer(state, action) {
    switch(action.type) {
        case 'sendLoginRequest': {
            return {
                ...state,
                isLoading: true
            };
        }
        case 'loginSucceeded': {
            let user = action.payload;
            return {
                ...state,
                isLoading: false,
                isLoggedIn: true,
                user: {
                    username: user.username,
                    userId: user.userId
                }
            };
        }
        case 'loginFailed' || 'logout': {
            return {
                ...state,
                isLoading: false,
                isLoggedIn: false,
                user: null
            };
        }
    }
}

function CurrentUserProvider({children}) {
    const [state, dispatch] = useReducer(currentUserReducer, {
        isLoggedIn: false,
        isLoading: false,
        user: null
    });
    
    const contextObject = {state, dispatch};
    return <UserContext.Provider value={contextObject}>{children}</UserContext.Provider>;
}

function useCurrentUser() {
    const context = useContext(UserContext);
    if (context === undefined) {
        throw new Error('useUser must be used within a CurrentUserProvider');
    }
    return context;
}

export { CurrentUserProvider, useCurrentUser }