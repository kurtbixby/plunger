import { makeRequest as makeBaseRequest } from "./Requests";

let accessToken = "";
const baseAddress = "https://localhost";
const port = "5213";
const combinedBaseUrl = baseAddress.concat(':', port);

function updateToken(newToken) {
    accessToken = newToken;
}

async function makePostRequest(url, payload, extHeaders = {}){
    return await makeRequest("POST", url, payload, extHeaders);
}

async function makePatchRequest(url, payload, extHeaders = {}){
    return await makeRequest("PATCH", url, payload, extHeaders);
}

async function makeDeleteRequest(url, extHeaders = {}){
    return await makeRequest("DELETE", url, {}, extHeaders);
}

async function makeRequest(method, url, payload, extHeaders = {}) {
    // Can handle refreshing the token is desired
    return makeBaseRequest(method, combinedBaseUrl + url, payload, {...extHeaders, "Authorization": `Bearer ${accessToken}`});
}

export default { updateToken, makePostRequest, makePatchRequest, makeDeleteRequest };