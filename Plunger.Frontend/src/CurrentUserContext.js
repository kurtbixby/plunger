import { createContext } from "react";
import { makePostRequest, makePatchRequest, makeDeleteRequest } from "./Requests";

const CurrentUserContext = createContext(null);

let authHeader = {"Authorization": `Bearer ${token}`};
/*
Handle user login/logout
Maintains current access token
Attaches token to each request
Exports functions for each call
 */

// What if a request fails or goes wrong
async function sendLoginRequest({identity, password}) {
    var loginRequest = { identity, password};
    return await makePostRequest("/api/users/login", loginRequest);
}

async function sendNewUserRequest(newUserRequest) {
    return await makePostRequest("/api/users", newUserRequest);
}

async function sendAddGameRequest(userId, addGameRequest) {
    return await makePostRequest(`/api/users/${userId}/collection`, addGameRequest, {...authHeader});
}

async function sendEditGameRequest(userId, itemId, editGameRequest) {
    return await makePatchRequest(`/api/users/${userId}/collection/${itemId}`, editGameRequest, {...authHeader});
}

async function sendDeleteGameRequest(userId, itemId) {
    return await makeDeleteRequest(`/api/users/${userId}/collection/${itemId}`, {...authHeader});
}

async function sendCreateListRequest(createListRequest) {
    return await makePostRequest("/api/lists", createListRequest,{...authHeader});
}

async function sendEditListRequest(listId, editListRequest) {
    return await makePatchRequest(`/api/lists/${listId}`, editListRequest, {...authHeader});
}

// Delete List is unimplemented on the backend
async function sendDeleteListRequest(listId, editListRequest) {
    return await makeDeleteRequest(`/api/lists/${listId}`, {...authHeader});
}

async function sendAddGameStatusRequest(userId, addGameStatusRequest) {
    return await makePostRequest(`/api/users/${userId}/games`, addGameStatusRequest, {...authHeader});
}

async function sendEditGameStatusRequest(userId, gameId, editGameStatusRequest) {
    return await makePatchRequest(`/api/users/${userId}/games/${gameId}`, editGameStatusRequest, {...authHeader});
}

// Delete GameStatus is unimplemented on the backend
async function sendDeleteGameStatusRequest(userId, gameId) {
    return await makeDeleteRequest(`/api/users/${userId}/games/${gameId}`, {...authHeader});
}

function objIsEmpty(obj) {
    return Object.keys(obj).length === 0;
}

export default CurrentUserContext;
