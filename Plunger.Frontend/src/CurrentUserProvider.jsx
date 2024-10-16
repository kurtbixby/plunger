import { createContext, useContext, useReducer, useEffect } from "react";
import TokenManagement from "./TokenManagement.js";
import APICalls from "./APICalls.js";

const UserContext = createContext(null);

function currentUserReducer(state, action) {
  switch (action.type) {
    case "sendLoginRequest": {
      return {
        ...state,
        isLoading: true,
      };
    }
    case "loginSucceeded": {
      let user = action.payload;
      return {
        ...state,
        isLoading: false,
        isLoggedIn: true,
        user: {
          username: user.username,
          id: user.userId,
        },
      };
    }
    case "loginFailed" || "logout": {
      return {
        ...state,
        isLoading: false,
        isLoggedIn: false,
        user: null,
      };
    }
  }
}

function CurrentUserProvider({ initialState, children }) {
  const [state, dispatch] = useReducer(
    currentUserReducer,
    initialState ?? {
      isLoggedIn: false,
      isLoading: false,
      user: null,
    },
  );

  useEffect(() => {
    (async () => await initializeUser())();
  }, []);

  async function initializeUser() {
    const storedToken = TokenManagement.loadToken();
    if (storedToken === null) {
      return;
    }

    dispatch({ type: "sendLoginRequest" });
    const userDetails = await APICalls.sendTokenLoginRequest(storedToken);
    if (!userDetails) {
      dispatch({ type: "loginFailed" });
      return;
    }
    dispatch({
      type: "loginSucceeded",
      payload: { username: userDetails.userName, userId: userDetails.userId },
    });

    // setUserState({isLoggedIn: true, isLoading: false, user: {username: userDetails.userName, userId: userDetails.userId}});
  }

  const contextObject = { state, dispatch };
  return (
    <UserContext.Provider value={contextObject}>
      {children}
    </UserContext.Provider>
  );
}

function useCurrentUser() {
  const context = useContext(UserContext);
  if (context === undefined) {
    throw new Error("useUser must be used within a CurrentUserProvider");
  }
  return context;
}

export { CurrentUserProvider, useCurrentUser };
