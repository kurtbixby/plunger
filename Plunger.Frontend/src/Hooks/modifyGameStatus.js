import APICalls from "../APICalls.js";

async function modifyGameStatus(userId, gameId, editRequest) {
  await APICalls.sendEditGameStatusRequest(userId, gameId, editRequest);
}

export default modifyGameStatus;
