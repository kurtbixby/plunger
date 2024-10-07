import APICalls from "../APICalls.js";

async function fetchCollectionEntries({ queryKey }) {
    const [_key, {username, page}] = queryKey;
    
    return await APICalls.sendGetCollectionRequest(username);
}

export default fetchCollectionEntries;