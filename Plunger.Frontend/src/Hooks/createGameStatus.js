import APICalls from '../APICalls.js';

async function createGameStatus(userId, createRequest) {
    await APICalls.sendAddGameStatusRequest(userId, createRequest);
}

export default createGameStatus;