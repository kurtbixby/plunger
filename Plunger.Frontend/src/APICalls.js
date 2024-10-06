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
    return await UserRequestsWrapper.makePostRequest(`/api/users/${userId}/collection`, addGameRequest);
}

async function sendEditGameRequest(userId, itemId, editGameRequest) {
    return await UserRequestsWrapper.makePatchRequest(`/api/users/${userId}/collection/${itemId}`, editGameRequest);
}

async function sendDeleteGameRequest(userId, itemId) {
    return await UserRequestsWrapper.makeDeleteRequest(`/api/users/${userId}/collection/${itemId}`);
}

async function sendCreateListRequest(createListRequest) {
    return await UserRequestsWrapper.makePostRequest("/api/lists", createListRequest);
}

async function sendEditListRequest(listId, editListRequest) {
    return await UserRequestsWrapper.makePatchRequest(`/api/lists/${listId}`, editListRequest);
}

// Delete List is unimplemented on the backend
async function sendDeleteListRequest(listId, editListRequest) {
    return await UserRequestsWrapper.makeDeleteRequest(`/api/lists/${listId}`);
}

async function sendAddGameStatusRequest(userId, addGameStatusRequest) {
    return await UserRequestsWrapper.makePostRequest(`/api/users/${userId}/games`, addGameStatusRequest);
}

async function sendEditGameStatusRequest(userId, gameId, editGameStatusRequest) {
    return await UserRequestsWrapper.makePatchRequest(`/api/users/${userId}/games/${gameId}`, editGameStatusRequest);
}

// Delete GameStatus is unimplemented on the backend
async function sendDeleteGameStatusRequest(userId, gameId) {
    return await UserRequestsWrapper.makeDeleteRequest(`/api/users/${userId}/games/${gameId}`);
}

async function sendEditCollectionViewRequest(userId, editCollectionViewRequest) {
    return await UserRequestsWrapper.makePatchRequest(`/api/users/${userId}/collection`, editCollectionViewRequest)
}

async function sendGameSearchRequest(gameName) {
    return await UserRequestsWrapper.makeGetRequest(`/api/games?name=${gameName}`);
}

async function sendPlatformsRequest() {
    return await UserRequestsWrapper.makeGetRequest(`/api/info/platforms`);
}

async function sendGetCollectionRequest(username){
    return await UserRequestsWrapper.makeGetRequest(`/api/users/${username}/collection`);
}

async function sendGetHomePageLists() {
    return await UserRequestsWrapper.makeGetRequest(`/api/lists/homepage`);
}

export default { sendLoginRequest, sendTokenLoginRequest, sendNewUserRequest, sendAddGameRequest, sendEditGameRequest, sendDeleteGameRequest, sendCreateListRequest, sendEditListRequest, sendDeleteListRequest, sendAddGameStatusRequest, sendEditGameStatusRequest, sendDeleteGameStatusRequest, sendGameSearchRequest, sendPlatformsRequest, sendGetCollectionRequest, sendGetHomePageLists, sendEditCollectionViewRequest };
