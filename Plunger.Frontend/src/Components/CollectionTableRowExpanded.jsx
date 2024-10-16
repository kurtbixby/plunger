import Enums from "../Enums.js";
import TableSelect from "./TableSelect.jsx";
import UIConstants from "../UIConstants.js";
import { useState } from "react";
import TableCurrency from "./TableCurrency.jsx";
import TableDate from "./TableDate.jsx";
import TableCheckbox from "./TableCheckbox.jsx";
import TableTime from "./TableTime.jsx";
import APICalls from "../APICalls.js";
import { useCurrentUser } from "../CurrentUserProvider.jsx";

function CollectionTableRowExpanded(props) {
  const { collectionEntry } = props;
  const {
    state: { user: currentUser },
  } = useCurrentUser();
  const [isEditable, setIsEditable] = useState(false);
  const [formState, setFormState] = useState({
    platform: collectionEntry.platform.id,
    region: collectionEntry.region.id,
    physicality: collectionEntry.physicality,
    purchasePrice: collectionEntry.purchasePrice,
    timeAcquired: collectionEntry.timeAcquired,
    timeAdded: collectionEntry.timeAdded,
    playState: collectionEntry.status?.playState,
    timePlayed: collectionEntry.status?.timePlayed,
    completed: collectionEntry.status?.completed,
    entryVersionId: collectionEntry.versionId,
  });
  const hasStatus = collectionEntry.status !== null;

  function toggleEdit() {
    setIsEditable(!isEditable);
  }

  function updateFormField(fieldName, value) {
    let newValue = formState;
    newValue[fieldName] = value;
    setFormState({
      ...newValue,
    });
  }

  async function submitForm(e) {
    e.preventDefault();

    const collectionGameEdits = {
      id: collectionEntry.id,
      platform: formState.platform,
      region: formState.region,
      physicality: formState.physicality,
      purchasePrice: formState.purchasePrice,
      timeAcquired: formState.timeAcquired,
      versionId: collectionEntry.versionId,
    };

    const gameStatusEdits = {
      id: collectionEntry.status?.id,
      timestamp: new Date(Date.now()).toISOString(),
      playState: formState.playState,
      timePlayed: formState.timePlayed,
      completed: formState.completed,
      versionId: collectionEntry.status?.versionId,
    };

    const collectionEditsRequest = {
      collectionGameEdits: collectionGameEdits,
      gameStatusEdits: hasStatus ? gameStatusEdits : null,
    };

    await APICalls.sendEditCollectionViewRequest(
      currentUser.id,
      collectionEditsRequest,
    );
  }

  return (
    <div className="mb-3 bg-fuchsia-400 pb-2">
      <div className="relative">
        <p className="mb-2 text-center">{collectionEntry.game.name}</p>
        <button className="absolute top-0 right-2" onClick={toggleEdit}>
          ✏️
        </button>
      </div>
      <form>
        <div className="flex text-center">
          {/* Left panel with artwork */}
          <div className="shrink basis-1/3">{/* Cover */}</div>
          {/* Right panel with details */}
          <div className="basis-2/3">
            <div className="flex">
              <div className="grow basis-1/3 flex flex-col justify-evenly">
                <TableSelect
                  isEditable={isEditable}
                  name="Platform"
                  textValue={
                    collectionEntry.game.platforms.find(
                      (p) => p.id === formState.platform,
                    ).name
                  }
                  value={formState.platform}
                  options={collectionEntry.game.platforms}
                  onSelect={(e) => updateFormField("platform", +e.target.value)}
                />
                <TableSelect
                  isEditable={isEditable}
                  name="Region"
                  textValue={collectionEntry.region.name}
                  value={formState.region}
                  options={UIConstants.Regions.list}
                  onSelect={(e) => updateFormField("region", +e.target.value)}
                />
                <TableSelect
                  isEditable={isEditable}
                  name="Physicality"
                  textValue={Enums.Physicality[formState.physicality]}
                  value={formState.physicality}
                  options={UIConstants.Physicalities.list}
                  onSelect={(e) =>
                    updateFormField("physicality", +e.target.value)
                  }
                />
              </div>
              <div className="grow basis-1/3 flex flex-col justify-evenly">
                <TableCurrency
                  isEditable={isEditable}
                  name="Price"
                  value={formState.purchasePrice}
                  onChange={(e) =>
                    updateFormField("purchasePrice", e.target.value)
                  }
                />
                <TableDate
                  isEditable={isEditable}
                  name="Date Acquired"
                  value={formState.timeAcquired}
                  onChange={(e) =>
                    updateFormField("timeAcquired", e.target.value)
                  }
                />
                <TableDate
                  isEditable={false}
                  name="Date Added"
                  value={formState.timeAdded}
                />
              </div>
              {hasStatus && (
                <div className="grow basis-1/3 flex flex-col justify-evenly">
                  <TableCheckbox
                    isEditable={isEditable}
                    name="Completed"
                    value={formState.completed}
                    onChange={(e) =>
                      updateFormField("completed", e.target.checked)
                    }
                  />
                  <TableSelect
                    isEditable={isEditable}
                    name="Current Status"
                    textValue={Enums.PlayStates[formState.playState]}
                    value={formState.playState}
                    options={UIConstants.PlayStates.list}
                    onSelect={(e) =>
                      updateFormField("playState", +e.target.value)
                    }
                  />
                  <TableTime
                    isEditable={isEditable}
                    name="Current Hours"
                    value={formState.timePlayed}
                    onChange={(e) =>
                      updateFormField("timePlayed", e.target.value)
                    }
                  />
                </div>
              )}
            </div>
            {isEditable && (
              <div>
                <button onClick={submitForm}>Save Changes</button>
              </div>
            )}
          </div>
        </div>
      </form>
    </div>
  );
}

export default CollectionTableRowExpanded;
