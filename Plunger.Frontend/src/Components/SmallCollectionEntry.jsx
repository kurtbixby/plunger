import { dateFormat, formatCurrency } from "../Utils.js";
import Enums from "../Enums.js";
import SmallRowElement from "./SmallRowElement.jsx";

function SmallCollectionEntry(props) {
  const {
    game: { collectionEntries, game },
  } = props;

  function saveChanges(event) {
    event.preventDefault();
    console.log("Saving Changes");
  }

  return (
    <div className="collectionEntry">
      {collectionEntries.map((entry) => (
        <div className="m-1 bg-amber-200 flex content-around" key={entry.id}>
          <SmallRowElement label={"Platform"} body={entry.platform.name} />
          <SmallRowElement label={"Region"} body={entry.region.name} />
          <SmallRowElement
            label={"Physicality"}
            body={Enums.Physicality[entry.physicality]}
          />
          <SmallRowElement
            label={"Date Acquired"}
            body={dateFormat(entry.timeAcquired)}
          />
          {entry.purchasePrice && (
            <SmallRowElement
              label={"Purchase Price"}
              body={formatCurrency(entry.purchasePrice)}
            />
          )}
          <SmallRowElement
            label={"Date Added"}
            body={dateFormat(entry.timeAdded)}
          />
        </div>
      ))}
      {/*<form onSubmit={saveChanges}>*/}
      {/*    <div>*/}
      {/*        <p>Current Status: </p>*/}
      {/*        <WrappedSelect name={"status"} contents={UIConstants.PlayStates.list} value={playStatus}*/}
      {/*                       onSelect={setPlayStatus}/>*/}
      {/*    </div>*/}
      {/*    <label htmlFor="currentHours">Hours Played:</label>*/}
      {/*    <input*/}
      {/*        name="currentHours"*/}
      {/*        value={currentHours}*/}
      {/*        onChange={(e) => {*/}
      {/*            let newValue = changeNumericTextField(e.target.value);*/}
      {/*            if (e.target.value === '') {*/}
      {/*                setCurrentHours('')*/}
      {/*            } else if (String(newValue) === e.target.value) {*/}
      {/*                setCurrentHours(newValue)*/}
      {/*            } else if (isPartialDecimal(newValue)) {*/}
      {/*                setCurrentHours(newValue)*/}
      {/*            }*/}
      {/*        }*/}
      {/*        }*/}
      {/*    />*/}
      {/*    <button type="submit">Save Changes</button>*/}
      {/*</form>*/}
    </div>
  );
}

export default SmallCollectionEntry;
