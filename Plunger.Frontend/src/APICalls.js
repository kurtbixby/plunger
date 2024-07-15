import UserRequestsWrapper from "./UserRequestsWrapper";
import TokenManagement from "./TokenManagement"

/*
Handle user login/logout
Maintains current access token
Attaches token to each request
Exports functions for each call
 */

// What if a request fails or goes wrong
async function sendLoginRequest({identity, password}) {
    let loginRequest = { identity, password};
    let response = await UserRequestsWrapper.makePostRequest("/api/users/login", loginRequest, {}, false);
    let { userDetails, token } = response;
    UserRequestsWrapper.updateToken(token);
    TokenManagement.storeToken(token);
    return userDetails;
}

async function sendTokenLoginRequest(token) {
    try {
        UserRequestsWrapper.updateToken(token);
        return await UserRequestsWrapper.makeGetRequest("/api/users/tokenLogin");
    } catch (error) {
        UserRequestsWrapper.clearToken();
        throw error;
    }
}

async function sendNewUserRequest(newUserRequest) {
    return await UserRequestsWrapper.makePostRequest("/api/users", newUserRequest);
}

async function sendAddGameRequest(userId, addGameRequest) {
    return await UserRequestsWrapper.makePostRequest(`/api/users/${userId}/collection`, addGameRequest, {...authHeader});
}

async function sendEditGameRequest(userId, itemId, editGameRequest) {
    return await UserRequestsWrapper.makePatchRequest(`/api/users/${userId}/collection/${itemId}`, editGameRequest, {...authHeader});
}

async function sendDeleteGameRequest(userId, itemId) {
    return await UserRequestsWrapper.makeDeleteRequest(`/api/users/${userId}/collection/${itemId}`, {...authHeader});
}

async function sendCreateListRequest(createListRequest) {
    return await UserRequestsWrapper.makePostRequest("/api/lists", createListRequest,{...authHeader});
}

async function sendEditListRequest(listId, editListRequest) {
    return await UserRequestsWrapper.makePatchRequest(`/api/lists/${listId}`, editListRequest, {...authHeader});
}

// Delete List is unimplemented on the backend
async function sendDeleteListRequest(listId, editListRequest) {
    return await UserRequestsWrapper.makeDeleteRequest(`/api/lists/${listId}`, {...authHeader});
}

async function sendAddGameStatusRequest(userId, addGameStatusRequest) {
    return await UserRequestsWrapper.makePostRequest(`/api/users/${userId}/games`, addGameStatusRequest, {...authHeader});
}

async function sendEditGameStatusRequest(userId, gameId, editGameStatusRequest) {
    return await UserRequestsWrapper.makePatchRequest(`/api/users/${userId}/games/${gameId}`, editGameStatusRequest, {...authHeader});
}

// Delete GameStatus is unimplemented on the backend
async function sendDeleteGameStatusRequest(userId, gameId) {
    return await UserRequestsWrapper.makeDeleteRequest(`/api/users/${userId}/games/${gameId}`, {...authHeader});
}

export default { sendLoginRequest, sendTokenLoginRequest, sendNewUserRequest, sendAddGameRequest, sendEditGameRequest, sendDeleteGameRequest, sendCreateListRequest, sendEditListRequest, sendDeleteListRequest, sendAddGameStatusRequest, sendEditGameStatusRequest, sendDeleteGameStatusRequest };
