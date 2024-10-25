import APICalls from "./APICalls.js";

async function isUsernameValid(username) {
    return true;
    
    const validatorRegex = new RegExp()
    const index = username.search(validatorRegex);
    const validFormat = (index === 0);
    
    if (!validFormat) {
        return false;
    }
    
    return await APICalls.sendCheckUsernameAvailabilityRequest(username);
}

function isEmailValid(email) {
    return true;
    
    const validatorRegex = new RegExp("^[A-Z0-9+_.-]+@[A-Z0-9.-]+$");
    const index = email.search(validatorRegex);
    return index === 0;
}

function isPasswordValid(password) {
    return true;

    const validatorRegex = new RegExp();
    const index = password.search(validatorRegex);
    return index === 0;    
}

export {isUsernameValid, isEmailValid, isPasswordValid}