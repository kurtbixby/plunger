import { useState } from "react";
import { useQuery } from "@tanstack/react-query";
import CollapsibleListItem from "../Components/CollapsibleListItem";
import fetchNowPlaying from "../Hooks/fetchNowPlaying";
import SmallGameStatus from "../Components/SmallGameStatus";
import { useCurrentUser } from "../CurrentUserProvider.jsx";

function NowPlayingWidget() {
  const { state: { user: currentUser } } = useCurrentUser();
  const [openedItemId, setOpenedItemId] = useState(NaN);

  const results = useQuery({
    queryKey: ["nowPlaying", currentUser === null ? 0 : currentUser.userId],
    queryFn: fetchNowPlaying,
  });

  function toggleItem(itemId) {
    if (itemId !== openedItemId) {
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
            isOpen={game.id === openedItemId}
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
