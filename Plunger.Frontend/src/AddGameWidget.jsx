import { useContext, useState } from "react";
import CurrentUserContext from "./CurrentUserContext";

function AddGameWidget() {
  const [currentUser] = useContext(CurrentUserContext);
  const [state, setState] = useState({
    gameName: "",
    platform: "",
    version: "",
    price: NaN,
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
      <label>
        Unspecified
        <input type="radio" name="tangibleOption" />
      </label>
      <label>
        Physical
        <input type="radio" name="tangibleOption" />
      </label>
      <label>
        Digital
        <input type="radio" name="tangibleOption" />
      </label>
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
