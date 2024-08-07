import {useContext, useEffect, useState, useRef} from "react";
import APICalls from "./APICalls.js";
import CurrentUserContext from "./CurrentUserContext";
import SearchDropDown from "./SearchDropDown";
import WrappedSelect from "./WrappedSelect";

function AddGameWidget() {
  const [currentUser] = useContext(CurrentUserContext);
  const [formState, setFormState] = useState({
    gameName: "",
    gameId: NaN,
    platform: "",
    platformId: NaN,
    version: "",
    price: NaN,
    tangibility: "",
    region: "",
    dateAcquired: todaysDateString(),
  });
  
  const [selectedGame, setSelectedGame] = useState({});
  const [selectedPlatformObject, setSelectedPlatformObject] = useState({});
  const [selectedPlatformId, setSelectedPlatformId] = useState("");
  const [searchResults, setSearchResults] = useState([]);
  
  // This could be somewhat hardcoded, definitely cached
  const [platforms, setPlatforms] = useState([]);

    useEffect(() => {
        (async () => {
            if (formState.gameName.length < 2) {
                return;
            }
            
            // make search game request
            const searchResponse = await APICalls.sendGameSearchRequest(formState.gameName);
            setSearchResults(searchResponse);
        })();
    }, [formState.gameName]);
    
    useEffect(() => {
        (async () => {
            const platformsResponse = await APICalls.sendPlatformsRequest();
            setPlatforms(platformsResponse);
        })()
    }, []);

  function todaysDateString() {
    const date = new Date().toISOString();
    const dateString = date.split("T")[0];

    return dateString;
  }

  function editFormValue(name, value) {
    setFormState({
      ...formState,
      [name]: value,
    });
  }
  
  function editFormValues(valuesDict) {
      setFormState({
          ...formState,
          ...valuesDict
      });
    }

  async function submitAddGame(event) {
    event.preventDefault();
    
    const addGameRequest = {
        gameid: formState.gameId,
        platformid: formState.platformId,
        regionid: formState.region,
        timeacquired: formState.dateAcquired,
        physicality: formState.tangibility
    };
    
    console.log(formState);
    console.log(addGameRequest);
    await APICalls.sendAddGameRequest(addGameRequest);
  }
  
  function validateFormInputs() {
      
  }
    
    function selectGame(game) {
        // Fill in fields
        editFormValues(
            {
                gameName: game.name,
                gameId: game.id
            }
        );
        
        // Set the game
        setSelectedGame(game);
        
        // Set the default region
        setFormState("region", game.regions[0]);
        
        // Set the default platform
        selectPlatformId(game.platforms[0].id);
    }
    
    function selectPlatformObject(platform) {
      console.log(platform.name);
      
      editFormValues(
          {
              platform: platform.name,
              platformId: platform.id
          }
      );
      
      // editFormValue("platform", platform.name);
      
      setSelectedPlatformObject(platform);
    }
    
    function selectPlatformId(value) {
      console.log(value);
      editFormValue("platformId", value);
      setSelectedPlatformId(value);
    }

  return (
      <div className="bg-purple-500">
          <h2>Add Game</h2>
          <form onSubmit={submitAddGame}>
              <div>
                  <label htmlFor="gameName">Game Name:</label>
                  <div>
                      <SearchDropDown onTextChange={(e) => editFormValue(e.target.name, e.target.value)} onItemSelect={selectGame}
                                      searchResults={searchResults} name={"gameName"} value={formState.gameName}/>
                  </div>
              </div>
              <div>
                  {/*<label htmlFor="platform">Platform:</label>*/}
                  {/*/!* Roll my own dropdown >:( *!/*/}
                  {/*/!* Filter for possible platforms *!/*/}
                  {/*/!* What about validity going into db? *!/*/}
                  {/*<input*/}
                  {/*    name="platform"*/}
                  {/*    type="text"*/}
                  {/*    value={formState.platform}*/}
                  {/*    onChange={(e) => editFormValue(e.target.name, e.target.value)}*/}
                  {/*/>*/}
                <label htmlFor="platformDropDown">Platform:</label>
                <div>
                    {/*<SelectDropDown contents={selectedGame?.platforms ?? platforms} value={formState.platform} initialMessage={"Select a platform"} onSelect={selectPlatform}/>*/}
                    <WrappedSelect name={"platform"} contents={selectedGame?.platforms ?? platforms} value={formState.platformId} onSelect={selectPlatformId}/>
                </div>
              </div>
              {/*<div>*/}
              {/*    <label htmlFor="version">Version:</label>*/}
              {/*    <input*/}
              {/*        name="version"*/}
              {/*        type="text"*/}
              {/*        value={formState.version}*/}
              {/*        onChange={(e) => editFormValue(e.target.name, e.target.value)}*/}
              {/*    />*/}
              {/*</div>*/}
              <div>
                  <label htmlFor="region">Region:</label>
                  <WrappedSelect name={"region"} contents={selectedGame?.regions ?? []} value={formState.region} onSelect={value => editFormValue("region", value)}/>
                  {/*<input*/}
                  {/*    name="region"*/}
                  {/*    type="text"*/}
                  {/*    value={formState.region}*/}
                  {/*    onChange={(e) => editFormValue(e.target.name, e.target.value)}*/}
                  {/*/>*/}
              </div>
    
              <div>
                  <label>Tangibility:</label>
              </div>
              <div>
                  <input type="radio" id="unspecified" name="tangibility" value=""
                         onChange={(e) => editFormValue(e.target.name, e.target.value)}/>
                  <label htmlFor="unspecified">Unspecified</label>
              </div>
              <div>
                  <input type="radio" id="physical" name="tangibility" value="physical"
                         onChange={(e) => editFormValue(e.target.name, e.target.value)}/>
                  <label htmlFor="physical">Physical</label>
              </div>
              <div>
                  <input type="radio" id="digital" name="tangibility" value="digital"
                         onChange={(e) => editFormValue(e.target.name, e.target.value)}/>
                  <label htmlFor="digital">Digital</label>
              </div>
    
              <div>
                  <label htmlFor="price">Price:</label>
                  <input
                      name="price"
                      type="text"
                      value={formState.price ? String(formState.price) : ""}
                      onChange={(e) => editFormValue(e.target.name, e.target.value)}
                  />
              </div>
              <div>
                  <label htmlFor="dateAcquired">Date Acquired:</label>
                  <input
                      name="dateAcquired"
                      type="date"
                      value={formState.dateAcquired}
                      onChange={(e) => editFormValue(e.target.name, e.target.value)}
                  />
              </div>
              <button type="submit">Add Game</button>
          </form>
      </div>
  );
}

export default AddGameWidget;
