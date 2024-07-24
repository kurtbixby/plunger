import {useContext, useEffect, useState, useRef} from "react";
import APICalls from "./APICalls.js";
import CurrentUserContext from "./CurrentUserContext";
import {useOnClickOutside} from "usehooks-ts";

function AddGameWidget() {
  const [currentUser] = useContext(CurrentUserContext);
  const [formState, setFormState] = useState({
    gameName: "",
    platform: "",
    version: "",
    price: NaN,
    tangibility: "",
    region: "",
    dateAcquired: todaysDateString(),
  });
  
  const [gameName, setGameName] = useState("");
  const [chosenGame, setChosenGame] = useState({});
  const [searchResults, setSearchResults] = useState([]);
  const [isSearchDropDownOpen, setIsSearchDropDownOpen] = useState(false);
  
  // if focused = isOpen

    useEffect(() => {
        (async () => {
            if (gameName.length < 2) {
                return;
            }
            
            // make search game request
            const searchResponse = await APICalls.sendGameSearchRequest(gameName);
            setSearchResults(searchResponse);
        })();
    }, [gameName]);

    
    // function useOutsideClick(callback) {
    //     const ref = useRef();
    //    
    //     useEffect(() => {
    //         const handleClick = (event) => {
    //             console.log("Handling Click");
    //             if (ref.current && ref.current !== event.target) {
    //                 console.log("About to close");
    //                 callback();
    //             }
    //         }
    //    
    //         document.addEventListener('click', handleClick, true);
    //    
    //         return () => {
    //             document.removeEventListener('click', handleClick, true);
    //         }
    //     }, [ref]);
    //    
    //     return ref;
    // }
    // const ref = useOutsideClick(closeDropDown);
    
    const ref = useRef(null);
    useOnClickOutside(ref, closeDropDown);
    
    function handleSafeClick(event) {
        event.stopPropagation();
    }

  function todaysDateString() {
    const date = new Date().toISOString();
    const dateString = date.split("T")[0];

    return dateString;
  }

  function editValue(name, value) {
    setFormState({
      ...formState,
      [name]: value,
    });
  }

  function submitAddGame(event) {
    event.preventDefault();
    
    console.log(formState);
  }
  
  function openDropDown() {
      setIsSearchDropDownOpen(true);
  }
  function closeDropDown() {
      setIsSearchDropDownOpen(false);
  }
  
  function validateFormInputs() {
      
  }

  // function editName(newName) {
  //   setState({
  //     ...state,

  //   })
  //   state.gameName = newName;
  // }

  // function selectPlatform(newPlatform) {
  //   state.platform = newPlatform;
  // }

  // function selectVersion(newVersion) {
  //   state.version = newVersion;
  // }

  // function editPrice(newPrice) {
  //   state.price = newPrice;
  // }

  // function editDate(newDate) {
  //   state.date = newDate;
  // }
    
    function selectGame(game) {
        console.log("Selected: " + game.name);
    }

  return (
      <form className="bg-purple-500" onSubmit={submitAddGame}>
          <div>
              <label htmlFor="gameName">Game Name:</label>
              <div 
                   ref={ref}>
                  <p>TEST TAG</p>
                  <input
                      name="gameName"
                      type="text"
                      value={gameName}
                      onChange={(e) => setGameName(e.target.value)}
                      onClick={openDropDown}
                  />
                  <div>
                      {isSearchDropDownOpen && (
                          <ul>
                              {searchResults.slice(0,5).map((game) => (
                                  <li className={"hover:bg-amber-400"} key={game.id} onClick={() => selectGame(game)}>{game.name}</li>
                              ))}
                          </ul>
                      )}
                  </div>
              </div>
          </div>
          <div>
              <label htmlFor="platform">Platform:</label>
              <input
                  name="platform"
                  type="text"
                  value={formState.platform}
                  onChange={(e) => editValue(e.target.name, e.target.value)}
              />
          </div>
          <div>
              <label htmlFor="version">Version:</label>
              <input
                  name="version"
                  type="text"
                  value={formState.version}
                  onChange={(e) => editValue(e.target.name, e.target.value)}
              />
          </div>
          <div>
              <label htmlFor="region">Region:</label>
              <input
                  name="region"
                  type="text"
                  value={formState.region}
                  onChange={(e) => editValue(e.target.name, e.target.value)}
              />
          </div>

          <div>
              <label>Tangibility:</label>
          </div>
          <div>
              <input type="radio" id="unspecified" name="tangibility" value="unspecified"
                     onChange={(e) => editValue(e.target.name, e.target.value)}/>
              <label htmlFor="unspecified">Unspecified</label>
          </div>
          <div>
              <input type="radio" id="physical" name="tangibility" value="physical"
                     onChange={(e) => editValue(e.target.name, e.target.value)}/>
              <label htmlFor="physical">Physical</label>
          </div>
          <div>
              <input type="radio" id="digital" name="tangibility" value="digital"
                     onChange={(e) => editValue(e.target.name, e.target.value)}/>
              <label htmlFor="digital">Digital</label>
          </div>

          <div>
              <label htmlFor="price">Price:</label>
              <input
                  name="price"
                  type="text"
                  value={formState.price ? String(formState.price) : ""}
                  onChange={(e) => editValue(e.target.name, e.target.value)}
              />
          </div>
          <div>
              <label htmlFor="dateAcquired">Date Acquired:</label>
              <input
                  name="dateAcquired"
                  type="date"
                  value={formState.dateAcquired}
                  onChange={(e) => editValue(e.target.name, e.target.value)}
              />
          </div>
          <button type="submit">Add Game</button>
      </form>
  );
}

export default AddGameWidget;
