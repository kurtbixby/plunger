import { objIsEmpty } from "./Utils.js";

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
    let fetchOptions = {
        method: method,
        credentials: "include",
        headers: {
            "Content-Type": "application/json",
            ...extHeaders
        }
    };
    if (!objIsEmpty(payload)) {
        fetchOptions.body = JSON.stringify(payload);
    }
    console.log(url);
    let response = await fetch(url, fetchOptions);
    return await response.json();
}

export { makePostRequest, makePatchRequest, makeDeleteRequest, makeRequest }