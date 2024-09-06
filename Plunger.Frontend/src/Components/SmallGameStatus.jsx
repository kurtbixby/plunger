import { useState } from "react";
import WrappedSelect from "./WrappedSelect.jsx";
import UIConstants from "../UIConstants.js"

function SmallGameStatus(props) {
  const { game } = props;
  
  const [currentHours, setCurrentHours] = useState(game.currentHours ?? 0);
  
  const [playStatus, setPlayStatus] = useState(game.playStatus ?? 0);
  
  function changeNumericTextField(text) {
    const unwantedFilter = /[^.\d]/;
    let filteredText = text.replace(unwantedFilter, '');
    let decimalIdx = filteredText.indexOf('.');
    if (decimalIdx !== -1) {   
        filteredText = filteredText.replaceAll('.', '');
        filteredText = filteredText.substring(0, decimalIdx) + '.' + filteredText.substring(decimalIdx);
    }
    if (!isNaN(filteredText)) {
        if (decimalIdx === -1) {
            return filteredText; // No decimal, is an integer
        }

        return filteredText.substring(0, Math.min(filteredText.length, decimalIdx + 1 + 2));
    }
    else {
      return text;
    }
  }
  
  function isPartialDecimal(numberText) {
      if (isNaN(numberText)) {
          return false;
      }
      
      return numberText.split('.').length === 2;
  }

  return (
    <div className="grid grid-cols-2">
      <img src={game.boxart} alt="" />
      <div className="currentStatus">
        <p>Date Started: {game.playState?.startTime}</p>
        <p>Date Acquired: {game.dateAcquired}</p>
        <p>Platform: {game.platform?.name}</p>
        <form>
          <div>
            <p>Current Status: </p>
            <WrappedSelect name={"status"} contents={UIConstants.PlayStates.list} value={playStatus} onSelect={setPlayStatus} />
          </div>
          <label htmlFor="currentHours">Current Hours:</label>
          <input
              name="currentHours"
              value={currentHours}
              onChange={(e) => {
                    let newValue = changeNumericTextField(e.target.value);
                    if (e.target.value === '') {
                      setCurrentHours('')
                    } else if (String(newValue) === e.target.value) {
                      setCurrentHours(newValue)
                    } else if (isPartialDecimal(newValue)) {
                        setCurrentHours(newValue)
                    }
                  }
                }
          />
        </form>
      </div>
    </div>
  );
}

export default SmallGameStatus;
