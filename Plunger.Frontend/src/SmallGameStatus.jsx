import { useState } from "react";

function SmallGameStatus({ game }) {
  const [currentHours, setCurrentHours] = useState(
    game.currentHours ? game.currentHours : NaN,
  );

  return (
    <div className="grid grid-cols-2">
      <img src={game.boxart} alt="" />
      <div className="currentStatus">
        <p>Date Started: </p>
        <p>Date Acquired: </p>
        <p>Platform: </p>
        <form>
          <label htmlFor="currentHours">Current Hours:</label>
          <input
            name="currentHours"
            type="number"
            value={String(currentHours)}
            onChange={(e) => setCurrentHours(e.target.value)}
          />
        </form>
      </div>
    </div>
  );
}

export default SmallGameStatus;
