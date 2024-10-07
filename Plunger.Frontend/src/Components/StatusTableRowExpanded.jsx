import TableSelect from "./TableSelect.jsx";
import UIConstants from "../UIConstants.js";
import Enums from "../Enums.js";
import TableDate from "./TableDate.jsx";
import TableCheckbox from "./TableCheckbox.jsx";
import TableTime from "./TableTime.jsx";
import {useCurrentUser} from "../CurrentUserProvider.jsx";

function StatusTableRowExpanded(props) {
    const {gameStatus} = props;
    const { state: {user: currentUser} } = useCurrentUser();
    const [isEditable, setIsEditable] = useState(false);
    const [formState, setFormState] = useState({
        playState: gameStatus.playState,
        timePlayed: gameStatus.timePlayed,
        completed: gameStatus.completed,
        entryVersionId: gameStatus.versionId,
        timeStarted: gameStatus.timeStarted,
        timeCompleted: gameStatus.timeCompleted
        
        // platform: gameStatus.platform.id,
        // region: gameStatus.region.id,
        // physicality: gameStatus.physicality,
        // purchasePrice: gameStatus.purchasePrice,
        // timeAcquired: gameStatus.timeAcquired,
        // timeAdded: gameStatus.timeAdded,
    });
    const [isCollectionOpen, setIsCollectionOpen] = useState(false);
    
    const hasCollection = gameStatus.collectionEntries != null;
    
    return <div className="mb-3 bg-fuchsia-400 pb-2">
        <div className="relative">
            <p className="mb-2 text-center">{gameStatus.game.name}</p>
            <button className="absolute top-0 right-2" onClick={toggleEdit}>✏️</button>
        </div>
        <div className="flex text-center">
            {/* Left panel with artwork */}
            <div className="shrink basis-1/3">
                {/* Cover */}
            </div>
            {/* Right panel with details */}
            <form>
                <div className="basis-2/3">
                    <div className="flex">
                        <div className="grow basis-1/3 flex flex-col justify-evenly">
                            <TableCheckbox isEditable={isEditable} name="Completed" value={formState.completed}
                                           onChange={e => updateFormField("completed", e.target.checked)}/>
                            <TableSelect isEditable={isEditable} name="Current Status"
                                         textValue={Enums.PlayStates[formState.playState]} value={formState.playState}
                                         options={UIConstants.PlayStates.list}
                                         onSelect={e => updateFormField("playState", +(e.target.value))}/>
                            <TableTime isEditable={isEditable} name="Current Hours" value={formState.timePlayed}
                                       onChange={e => updateFormField("timePlayed", e.target.value)}/>
                        </div>
                        <div className="grow basis-1/3 flex flex-col justify-evenly">
                            <TableDate isEditable={isEditable} name="Date Started" value={formState.timeStarted}
                                       onChange={e => updateFormField("timeStarted", e.target.value)}/>
                            <TableDate isEditable={isEditable} name="Date Completed" value={formState.timeCompleted}
                                       onChange={e => updateFormField("timeCompleted", e.target.value)}/>
                        </div>
                    </div>
                    {isEditable && <div>
                        <button onClick={submitForm}>Save Changes</button>
                    </div>}
                </div>
            </form>
            {hasCollection && <div>
                <button onClick={setIsCollectionOpen(!isCollectionOpen)}>{!isCollectionOpen ? "Show versions in collection" : "Hide versions in collection"}</button>
            </div>}
        </div>
        {/*{(hasCollection && isCollectionOpen) && <StatusTableSmallCollectionEntries collectionEntries={gameStatus.collectionEntries}/>}*/}
    </div>
}

export default StatusTableRowExpanded;