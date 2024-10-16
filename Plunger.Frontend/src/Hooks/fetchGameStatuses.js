import APICalls from "../APICalls.js";

async function fetchGameStatuses({ queryKey }) {
  const [_key, { username, page }] = queryKey;

  return await APICalls.sendGetGameStatusesRequest(username);
}

export default fetchGameStatuses;
