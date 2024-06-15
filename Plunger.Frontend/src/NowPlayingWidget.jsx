import { useContext, useState } from "react";
import { useQuery } from "@tanstack/react-query";
import CurrentUserContext from "./CurrentUserContext";
import CollapsibleListItem from "./CollapsibleListItem";
import fetchNowPlaying from "./fetchNowPlaying";
import SmallGameStatus from "./SmallGameStatus";

function NowPlayingWidget() {
  const [currentUser] = useContext(CurrentUserContext);
  const [openedItemId, setOpenedItemId] = useState(NaN);

  const results = useQuery({
    queryKey: ["nowPlaying", currentUser],
    queryFn: fetchNowPlaying,
  });

  function toggleItem(itemId) {
    if (itemId != openedItemId) {
      setOpenedItemId(itemId);
    } else {
      setOpenedItemId(NaN);
    }
  }

  const nowPlaying = results?.data ?? null;

  return (
    <div className="bg-emerald-700">
      <div className="nowPlayingHeader">
        <h2>Now Playing</h2>
      </div>
      {nowPlaying == null ? (
        <p>Loading Now Playing</p>
      ) : (
        nowPlaying.entries.map((game) => (
          <CollapsibleListItem
            key={game.id}
            title={game.name}
            id={game.id}
            isOpen={game.id == openedItemId}
            toggleItem={toggleItem}
          >
            <SmallGameStatus game={game} />
          </CollapsibleListItem>
        ))
      )}
    </div>
  );
}

export default NowPlayingWidget;
