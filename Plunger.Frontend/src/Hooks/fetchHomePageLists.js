import APICalls from "../APICalls.js";

async function fetchHomePageLists() {
  return await APICalls.sendGetHomePageLists();
}

export default fetchHomePageLists;
