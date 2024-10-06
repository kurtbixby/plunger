import {useEffect, useState, useRef, useReducer} from "react";
import APICalls from "../APICalls.js";
import SearchDropDown from "../Components/SearchDropDown";
import WrappedSelect from "../Components/WrappedSelect";
import addGameReducer from "./addGameReducer.js";
import currency from "currency.js";
import {useCurrentUser} from "../CurrentUserProvider.jsx";
import CurrencyInput from "../Components/CurrencyInput.jsx";

function AddGameWidget() {
    const { state: {user: currentUser} } = useCurrentUser();
  const [formState, dispatch] = useReducer(addGameReducer, {
      game: {},
      gameName: "",
      gameId: NaN,
      platform: "",
      platformId: NaN,
      version: "",
      price: NaN,
      searchField: "",
      tangibility: "",
      region: "",
      dateAcquired: todaysDateString(),
  });
  
  const [searchResults, setSearchResults] = useState([]);
  
  // This could be somewhat hardcoded, definitely cached
  const [platforms, setPlatforms] = useState([]);

    useEffect(() => {
        (async () => {
            if (formState.searchField.length < 2) {
                return;
            }
            
            // make search game request
            const searchResponse = await APICalls.sendGameSearchRequest(formState.searchField);
            setSearchResults(searchResponse);
        })();
    }, [formState.searchField]);
    
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

  async function submitAddGame(event) {
    event.preventDefault();
    
    const addGameRequest = {
        gameid: formState.game.id,
        platformid: formState.platformId,
        regionid: formState.region,
        timeacquired: formState.dateAcquired,
        purchaseprice: formState.price,
        physicality: formState.tangibility
    };
    
    console.log(formState);
    console.log(addGameRequest);
    await APICalls.sendAddGameRequest(currentUser.userId, addGameRequest);
  }
  
  function validateFormInputs() {
      
  }
    
    function changeTextDispatch(typedText) {
      dispatch({
          type: "searchTextChanged",
          payload: typedText
      });
    }
    
    function selectGameDispatch(game) {
      dispatch({
          type: "gameSelected",
          payload: game
      });
    }
    
    function selectPlatformIdDispatch(platformId) {
      dispatch({
          type: "platformSelected",
          payload: platformId
      });
    }
    
    function selectRegionDispatch(regionId) {
      dispatch({
          type: "regionSelected",
          payload: regionId
      });
    }
    
    function selectTangibilityDispatch(tangibility) {
      dispatch({
          type: "tangibilitySelected",
          payload: tangibility
      });
    }
    
    function changePriceDispatch(priceNum) {
        dispatch({
            type: "priceChanged",
            payload: priceNum
        });
    }
    
    function selectDateDispatch(date) {
      dispatch({
          type: "dateSelected",
          payload: date
      });
    }

  return (
      <div className="bg-purple-500">
          <h2>Add Game</h2>
          <form onSubmit={submitAddGame}>
              <div>
                  <label htmlFor="gameName">Game Name:</label>
                  <div>
                      <SearchDropDown onTextChange={changeTextDispatch} onItemSelect={selectGameDispatch}
                                      searchResults={searchResults} name={"gameName"} value={formState.searchField}/>
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
                    <WrappedSelect name={"platform"} contents={formState.game?.platforms ?? platforms} value={formState.platformId} onSelect={e => selectPlatformIdDispatch(e.target.value)}/>
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
                  <WrappedSelect name={"region"} contents={formState.game?.regions ?? []} value={formState.region} onSelect={e => selectRegionDispatch(e.target.value)}/>
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
                  <input type="radio" id="unspecified" name="tangibility" value="" checked={formState.tangibility === ""}
                         onChange={(e) => selectTangibilityDispatch(e.target.value)}/>
                  <label htmlFor="unspecified">Unspecified</label>
              </div>
              <div>
                  <input type="radio" id="physical" name="tangibility" value="physical" checked={formState.tangibility === "physical"}
                         onChange={(e) => selectTangibilityDispatch(e.target.value)}/>
                  <label htmlFor="physical">Physical</label>
              </div>
              <div>
                  <input type="radio" id="digital" name="tangibility" value="digital" checked={formState.tangibility === "digital"}
                         onChange={(e) => selectTangibilityDispatch(e.target.value)}/>
                  <label htmlFor="digital">Digital</label>
              </div>
    
              <div>
                  <label htmlFor="price">Price:</label>
                  <CurrencyInput name="price" value={formState.price} onChange={changePriceDispatch}/>
              </div>
              <div>
                  <label htmlFor="dateAcquired">Date Acquired:</label>
                  <input
                      name="dateAcquired"
                      type="date"
                      value={formState.dateAcquired}
                      onChange={(e) => selectDateDispatch(e.target.value)}
                  />
              </div>
              <button type="submit">Add Game</button>
          </form>
      </div>
  );
}

export default AddGameWidget;
