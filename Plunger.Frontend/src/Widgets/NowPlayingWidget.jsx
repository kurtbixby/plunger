import { useState } from "react";
import { useQuery } from "@tanstack/react-query";
import CollapsibleListItem from "../Components/CollapsibleListItem";
import fetchNowPlaying from "../Hooks/fetchNowPlaying";
import SmallGameStatus from "../Components/SmallGameStatus";
import { useCurrentUser } from "../CurrentUserProvider.jsx";

function NowPlayingWidget(props) {
  const { list } = props;
  const {
    state: { user: currentUser },
  } = useCurrentUser();
  const [openedItemId, setOpenedItemId] = useState(NaN);

  // const results = useQuery({
  //   queryKey: ["nowPlaying", currentUser === null ? 0 : currentUser.userId],
  //   queryFn: fetchNowPlaying,
  // });

  function toggleItem(itemId) {
    if (itemId !== openedItemId) {
      setOpenedItemId(itemId);
    } else {
      setOpenedItemId(NaN);
    }
  }

  // const nowPlaying = results?.data ?? null;

  return (
    <div className="bg-emerald-700">
      <div className="nowPlayingHeader">
        <h2>Now Playing</h2>
      </div>
      {list == null ? (
        <p>Loading Now Playing</p>
      ) : (
        list.listEntries.map((listEntry) => (
          <CollapsibleListItem
            key={listEntry.id}
            title={listEntry.game.name}
            id={listEntry.id}
            isOpen={listEntry.id === openedItemId}
            toggleItem={toggleItem}
          >
            <SmallGameStatus game={listEntry} />
          </CollapsibleListItem>
        ))
      )}
    </div>
  );
}

export default NowPlayingWidget;
