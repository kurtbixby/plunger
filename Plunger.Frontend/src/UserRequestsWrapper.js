import { makeRequest as makeBaseRequest } from "./Requests";

let accessToken = "";
const baseUrl = import.meta.env.VITE_API_BASE_URL;

function updateToken(newToken) {
  accessToken = newToken;
}

function clearToken() {
  updateToken("");
}

async function makeGetRequest(url, extHeaders = {}, withAuth = true) {
  return await makeRequest("GET", url, "", extHeaders, withAuth);
}

async function makePostRequest(url, payload, extHeaders = {}, withAuth = true) {
  return await makeRequest("POST", url, payload, extHeaders, withAuth);
}

async function makePatchRequest(
  url,
  payload,
  extHeaders = {},
  withAuth = true,
) {
  return await makeRequest("PATCH", url, payload, extHeaders, withAuth);
}

async function makeDeleteRequest(url, extHeaders = {}, withAuth = true) {
  return await makeRequest("DELETE", url, {}, extHeaders, withAuth);
}

async function makeRequest(
  method,
  path,
  payload,
  extHeaders = {},
  withAuth = true,
) {
  // Can handle refreshing the token is desired
  if (withAuth) {
    extHeaders["Authorization"] = `Bearer ${accessToken}`;
  }
  return await makeBaseRequest(
    method,
      baseUrl + path,
    payload,
    extHeaders,
  );
}

export default {
  updateToken,
  clearToken,
  makeGetRequest,
  makePostRequest,
  makePatchRequest,
  makeDeleteRequest,
};
