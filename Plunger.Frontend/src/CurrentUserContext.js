import { createContext } from "react";

const CurrentUserContext = createContext(null);

/*
Handle user login/logout
Maintains current access token
Attaches token to each request
Exports functions for each call
 */
export default CurrentUserContext;
