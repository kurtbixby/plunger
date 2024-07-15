import { useContext, useState } from "react";
import CurrentUserContext from "./CurrentUserContext";

function AddGameWidget() {
  const [currentUser] = useContext(CurrentUserContext);
  const [state, setState] = useState({
    gameName: "",
    platform: "",
    version: "",
    price: NaN,
    tangibility: "",
    region: "",
    dateAcquired: todaysDateString(),
  });

  function todaysDateString() {
    const date = new Date().toISOString();
    const dateString = date.split("T")[0];

    return dateString;
  }

  function editValue(name, value) {
    setState({
      ...state,
      [name]: value,
    });
  }

  function submitAddGame(event) {
    event.preventDefault();
    
    console.log(state);
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

  return (
      <form className="bg-purple-500" onSubmit={submitAddGame}>
          <div>
              <label htmlFor="gameName">Game Name:</label>
              <input
                  name="gameName"
                  type="text"
                  value={state.gameName}
                  onChange={(e) => editValue(e.target.name, e.target.value)}
              />
          </div>
          <div>
              <label htmlFor="platform">Platform:</label>
              <input
                  name="platform"
                  type="text"
                  value={state.platform}
                  onChange={(e) => editValue(e.target.name, e.target.value)}
              />
          </div>
          <div>
              <label htmlFor="version">Version:</label>
              <input
                  name="version"
                  type="text"
                  value={state.version}
                  onChange={(e) => editValue(e.target.name, e.target.value)}
              />
          </div>
          <div>
              <label htmlFor="region">Region:</label>
              <input
                  name="region"
                  type="text"
                  value={state.region}
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
                  value={state.price ? String(state.price) : ""}
                  onChange={(e) => editValue(e.target.name, e.target.value)}
              />
          </div>
          <div>
              <label htmlFor="dateAcquired">Date Acquired:</label>
              <input
                  name="dateAcquired"
                  type="date"
                  value={state.dateAcquired}
                  onChange={(e) => editValue(e.target.name, e.target.value)}
              />
          </div>
          <button type="submit">Add Game</button>
      </form>
  );
}

export default AddGameWidget;
