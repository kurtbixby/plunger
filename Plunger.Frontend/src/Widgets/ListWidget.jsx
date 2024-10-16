import { useState } from "react";
import { useQuery } from "@tanstack/react-query";
import CollapsibleListItem from "../Components/CollapsibleListItem";
import { useCurrentUser } from "../CurrentUserProvider.jsx";
import SmallCollectionEntry from "../Components/SmallCollectionEntry.jsx";
import AddStatusWidget from "./AddStatusWidget.jsx";
import GameStatusWidget from "./GameStatusWidget.jsx";

function ListWidget(props) {
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

  return (
    <div className="bg-emerald-300">
      <div className="listWidgetHeader">
        <h2>{list.name}</h2>
      </div>
      {list == null ? (
        <p>Loading List</p>
      ) : (
        list.listEntries.map((listEntry) => (
          <CollapsibleListItem
            key={listEntry.id}
            title={listEntry.game.name}
            id={listEntry.id}
            isOpen={listEntry.id === openedItemId}
            toggleItem={toggleItem}
          >
            {listEntry.status ? (
              <GameStatusWidget game={listEntry} />
            ) : (
              <AddStatusWidget game={listEntry.game} />
            )}
            {listEntry.collectionEntries && (
              <SmallCollectionEntry game={listEntry} />
            )}
          </CollapsibleListItem>
        ))
      )}
    </div>
  );
}

export default ListWidget;
