const TOKEN_NAME = "token";

function loadToken() {
  let storedValue = sessionStorage.getItem(TOKEN_NAME);
  return storedValue;
}

function storeToken(token) {
  sessionStorage.setItem(TOKEN_NAME, token);
}

function clearToken() {
  sessionStorage.removeItem(TOKEN_NAME);
}

export default { loadToken, storeToken, clearToken };
